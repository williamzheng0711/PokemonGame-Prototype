using System;
using System.Collections.Generic;
using System.Text;

namespace GameModel
{
    public class Controller
    {

        private Controller instance;
        public Controller Main
        {
            get
            {
                if (instance == null)
                    instance = new Controller();
                return instance;
            }
        }

        private Controller()
        {
            
        }
    }
}
