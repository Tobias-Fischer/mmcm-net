using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCMLibrary.CVZ.CTPC
{
    public class NeuronIzhikevich
    {
        /// <summary>
        /// Inner classe dedicated to the connection between 2 neurons
        /// </summary>
        public class Connection
        {
            /// <summary>
            /// The list of spikes currently traveling on the connection. With the time remaining before arrival.
            /// </summary>
            List<int> spikeTrain = new List<int>();
            
            /// <summary>
            /// The time required to go through the connection, in ms (real time)
            /// </summary>
            public int transmitionDelay = 1;

            public double w = 1.0;

            /// <summary>
            /// The neuron targeted
            /// </summary>
            public NeuronIzhikevich target;

            public Connection(NeuronIzhikevich target, double transmitionDelay, double w)
            {
                this.target = target;
                this.w = w;
            }

            /// <summary>
            /// Deliver spikes
            /// </summary>
            public void update()
            {
                for (int i = 0; i < spikeTrain.Count;i++ )
                {
                    spikeTrain[i]--;
                    if (spikeTrain[i] == 0)
                    {
                        propagateSpike();
                    }
                }

                spikeTrain.Remove(0);
            }

            /// <summary>
            /// 
            /// </summary>
            public void loadSpike()
            {
                spikeTrain.Add(transmitionDelay);
            }

            /// <summary>
            /// Propagate directly the weight of the connection to the target neuron membrane potential
            /// </summary>
            private void propagateSpike()
            {
                target.stimulus += w;
            }

            /// <summary>
            /// Propagate a portion of activity measuring the similarity between the weight and an origin input, with an overall
            /// factor originating from the modality influence / modalitysize
            /// </summary>
            /// <param name="originInput"></param>
            public double propagateSimilarity(double originInput, double factor = 1.0)
            {
                double p = (1 - Math.Abs(w - originInput)) * factor;
                p = Math.Min(1.0, Math.Max(0.0, p)); // Just to be sure
                target.stimulus += p;
                return p;
            }
        }

        /// <summary>
        /// Is the frequency calculation on?
        /// </summary>
        public bool frequency_calculation_on = false;
        /// <summary>
        /// Used to store the spiking behavior in a window to calculate spiking frequency
        /// </summary>
        private bool[] frequency_calculation_buffer;
        /// <summary>
        /// size of the freq. calculation buffer
        /// </summary>
        private const int frequency_calculation_buffer_size = 100;

        /// <summary>
        /// Position on the map, for debugging purposes
        /// </summary>
        public int tag_x;

        /// <summary>
        ///  Position on the map, for debugging purposes
        /// </summary>
        public int tag_y;

        /// <summary>
        /// A list of all the lateral projections of this neuron
        /// </summary>
        public List<Connection> lateralProjections;

        /// <summary>
        /// A list of all the input coming in this neurons (not taking into account lateral ones)
        /// </summary>
        public List<Connection> inputsConnections;

        /// <summary>
        /// Time scale of the recovery variable u
        /// </summary>
        double a;
        
        /// <summary>
        /// Sensitivity of the recovery variable u
        /// </summary>
        double b;

        /// <summary>
        /// After-spike reset of the membrane potential by fast high-treshold
        /// </summary>
        double c;

        /// <summary>
        /// After-spike reset of the membrane potential by slow high-treshold
        /// </summary>
        double d;

        /// <summary>
        /// Membrane potential
        /// </summary>
        double v;
        public double V { get { return v; } }

        /// <summary>
        /// Membrane recovery
        /// </summary>
        double u;
        
        /// <summary>
        /// Spiking treshold. When the membrane potential reaches this treshold the neuron fires.
        /// </summary>
        double spikingTreshold;

        /// <summary>
        /// Buffer the stimulus until next update
        /// </summary>
        public double stimulus;

        private bool hasJustSpiked = false;
        /// <summary>
        /// Did the neuron spiked last update?
        /// </summary>
        public bool HasJustSpiked { get { return hasJustSpiked; } }

        public NeuronIzhikevich(double spikingTreshold = 30.0, double a = 0.02, double b = 0.2, double c = -65.0, double d = 2.0)
        {
            v = 0.0;
            u = 0.0;
            this.spikingTreshold = spikingTreshold;
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            lateralProjections = new List<Connection>();
            inputsConnections = new List<Connection>();

            frequency_calculation_buffer = new bool[frequency_calculation_buffer_size];
        }

        /// <summary>
        /// Update the neuron. Return true if the neuron spiked.
        /// </summary>
        /// <returns>Did the neuron spiked</returns>
        public bool update()
        {
            bool doesSpike = false;

            //Calculate membrane potential
            hasJustSpiked = false;
            v = v + (0.04 * Math.Pow(v, 2.0) + 5.0 * v + 140.0 - u + stimulus * spikingTreshold);
            u = u + a * (b * v - u);

            //Console.WriteLine("Neuron Stimulus: " + stimulus); 
            //Reset the stimulus for the next update
            stimulus = 0.0;

            //Spikes or not
            if (v > spikingTreshold)
            {
                hasJustSpiked = true;
                v = c;
                u = u + d;
                Parallel.ForEach(lateralProjections, to =>
                    {
                        to.loadSpike();
                    });
                doesSpike = true;
            }
            else
                doesSpike = false;

            if (frequency_calculation_on)
                updateFrequencyCalculationBuffer(doesSpike);

            return doesSpike;
        }

        private void updateFrequencyCalculationBuffer(bool val)
        {
            for (int i = 1; i < frequency_calculation_buffer_size; i++)
            {
                frequency_calculation_buffer[frequency_calculation_buffer_size - i] = frequency_calculation_buffer[frequency_calculation_buffer_size - (i + 1)];
            }
            frequency_calculation_buffer[0] = val;
        }

        public float GetSpikingFrequency()
        {
            int spikeCount = 0;
            for (int i = 0; i < frequency_calculation_buffer_size; i++)
            {
                if (frequency_calculation_buffer[i])
                    spikeCount++;
            }
            return spikeCount / (float)frequency_calculation_buffer_size;
        }
    }
}
