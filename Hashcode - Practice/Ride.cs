using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hashcode___Practice {
    public class Ride {
        public static int bonus;
        public static int totalSteps;
        public int Index { get; }
        public Tuple<int, int> StartLocation { get; }
        public Tuple<int, int> EndLocation{ get; }
        public int EarliestStart { get; }
        public int LatestFinish { get; }
        public int Raw_score { get; }

        public Ride(List<int> rideDeets, int ind) {
            Index = ind;
            StartLocation = new Tuple<int, int>(rideDeets[0], rideDeets[1]);
            EndLocation = new Tuple<int, int>(rideDeets[2], rideDeets[3]);
            EarliestStart = rideDeets[4];
            LatestFinish = rideDeets[5];
            Raw_score = SelfDriving.GetDist(StartLocation, EndLocation);
        }

        public int ScoreRide(Car car, int currentStep) {
            
            int score = 0;
            int rideStart;
            int carArrives = SelfDriving.GetDist(car.Location, StartLocation) + currentStep;

            if (carArrives <= EarliestStart) {
                score = bonus;
                rideStart = EarliestStart;
            } else {
                rideStart = carArrives;
            }


            int downTime = rideStart - currentStep;
            const int MAX_DOWN_TIME = 1000000000;
            score += (MAX_DOWN_TIME - downTime); //invert, so lower downtime gives higher score

            int rideFinish = rideStart + Raw_score;
            if(rideFinish<=LatestFinish && rideFinish <= totalSteps) {
                return score;
                //return downTime;
            } else {
                return 0;
            }
        }

    }
}