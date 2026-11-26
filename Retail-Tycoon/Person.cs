using System;
using System.Collections.Generic;
using System.Text;

namespace Retail_Tycoon
{
    internal class Person : Object
    {
        public float Hunger = 0;
        public float Thirst = 0;
        public float Health = 0;
        public Emotion Emotion = new Emotion(60, 20);
    }

    struct Emotion
    {
        /// <summary>
        /// The procentual level of joy. Negative joy is sadness.
        /// </summary>
        public float Joy = 0;
        /// <summary>
        /// The procentual level of boredom. Negative boredom is anger.
        /// </summary>
        public float Boredom = 0;

        public Emotion(float joy, float boredom)
        {
            Joy = joy;
            Boredom = boredom;
        }
    }
}
