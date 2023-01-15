using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public class SnakeLevelsData
    {
        #region Fields
        private int[] defaultFoodEachLevel;
        private int[] defaultBombsEachLevel;
        private float[] timesEachLevel;
        #endregion Fields

        #region Properties
        public int[] DefaultFoodEachLevel
        {
            get
            {
                return defaultFoodEachLevel;
            }
        }

        public int[] DefaultBombsEachLevel
        {
            get
            {
                return defaultBombsEachLevel;
            }
        }

        public float[] TimesEachLevel
        {
            get
            {
                return timesEachLevel;
            }
        }
        #endregion Properties

        public SnakeLevelsData(int[] defaultFoodEachLevel, int[] defaultBombsEachLevel, float[] timesEachLevel)
        {
            this.defaultFoodEachLevel = defaultFoodEachLevel;
            this.defaultBombsEachLevel = defaultBombsEachLevel;
            this.timesEachLevel = timesEachLevel;
        }
    }
}
