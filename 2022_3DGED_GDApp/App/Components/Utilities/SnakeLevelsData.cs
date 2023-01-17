using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    /// <summary>
    /// Updates Data for the snake game levels
    /// </summary>
    public class SnakeLevelsData
    {
        #region Fields
        private int[] defaultFoodEachLevel;
        private int[] defaultBombsEachLevel;
        private int[] maxScore;
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

        public int[] MaxScore
        {
            get
            {
                return maxScore;
            }
        }
        #endregion Properties

        public SnakeLevelsData(int[] defaultFoodEachLevel, int[] defaultBombsEachLevel, float[] timesEachLevel, int[] maxScore)
        {
            this.defaultFoodEachLevel = defaultFoodEachLevel;
            this.defaultBombsEachLevel = defaultBombsEachLevel;
            this.timesEachLevel = timesEachLevel;
            this.maxScore = maxScore;
        }
    }
}
