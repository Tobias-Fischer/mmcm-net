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
            private NeuronIzhikevich target;

            public Connection(ref NeuronIzhikevich target, double transmitionDelay, double w)
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
            /// factor originating from the modality influence
            /// </summary>
            /// <param name="originInput"></param>
            public double propagateSimilarity(double originInput, double modalityInfluence = 1.0)
            {
                double p = ( 1 - (w - originInput) ) * modalityInfluence;
                p = Math.Min(1.0, Math.Max(0.0, p));
                target.stimulus += p;
                return p;
            }
        }

        /// <summary>
        /// A list of all the projections of this neuron
        /// </summary>
        public List<Connection> projections;

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

        public NeuronIzhikevich(double spikingTreshold = 30.0, double a = 0.02, double b = 0.2, double c = -65.0, double d = 2.0)
        {
            v = 0.0;
            u = 0.0;
            this.spikingTreshold = spikingTreshold;
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            projections = new List<Connection>();
        }

        /// <summary>
        /// Update the neuron. Return true if the neuron spiked.
        /// </summary>
        /// <returns>Did the neuron spiked</returns>
        public bool update()
        {
            v = v + (0.04 * Math.Pow(v, 2.0) + 5.0 * v + 140.0 - u + stimulus);
            u = u + a * (b * v - u);

            if (v > spikingTreshold)
            {
                v = c;
                u = u + d;
                Parallel.ForEach(projections, to =>
                    {
                        to.loadSpike();
                    });
                return true;
            }
            else
                return false;
        }
    }
}
