using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Output
{
    public class GameOfLifeOutput
    {
        private List<String> buffer;
        private String output;
        private String constantOutput;
        private double elapsedSinceUpdate;

        public GameOfLifeOutput()
        {
            buffer = new List<String>();
            output = "";
            constantOutput = "";
            elapsedSinceUpdate = 0;
        }
        
        public void Write(String text)
        {
            buffer.Add(text);
        }

        public void Update(double elapsedInMilliseconds)
        {
            elapsedSinceUpdate += elapsedInMilliseconds;
            if ((output == "" || elapsedSinceUpdate >= 2000) && constantOutput == "")
            {
                elapsedSinceUpdate = 0;
                if (buffer.Count != 0)
                {
                    output = buffer[0];
                    buffer.Remove(buffer[0]);
                }
                else
                {
                    output = "";
                }
            }
        }

        public String GetOutput()
        {
            if (constantOutput != "")
            {
                return constantOutput;
            }
            else
            {
                return output;
            }
        }

        public void ConstantText(String text)
        {
            constantOutput = text;
        }

        public void RemoveConstantText()
        {
            constantOutput = "";
        }

    }
}
