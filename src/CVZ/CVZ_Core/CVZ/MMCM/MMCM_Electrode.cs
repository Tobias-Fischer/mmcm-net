using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVZ_Core
{
    public class MMCM_Electrode
    {
        /// <summary>
        /// Name of the electrode
        /// </summary>
        public string name;

        /// <summary>
        /// Value of the recorded activity
        /// </summary>
        public float activity = 0.0f;

        /// <summary>
        /// Value of the stimulation (can be used to excite or inhib)
        /// </summary>
        public float intensity = 0.0f;

        /// <summary>
        /// Radius of the stimulation (todo : change to a gaussian)
        /// </summary>
        public float radius;

        /// <summary>
        /// X coordinate of the neuron to which the electrode is attached
        /// </summary>
        public int x;

        /// <summary>
        /// Y coordinate of the neuron to which the electrode is attached
        /// </summary>
        public int y;

        /// <summary>
        /// Z coordinate of the neuron to which the electrode is attached
        /// </summary>
        public int z;
    }
}
