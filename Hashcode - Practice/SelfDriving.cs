using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hashcode___Practice {
    class SelfDriving {
        public static List<List<int>> Solve(int totalRows, int totalColumns, int noOfCars, int noOfRides, int bonus, int totalSteps, List<List<int>> rides) {

            int currentStep = 0;
            List<List<int>> assignedRides = new List<List<int>>();
            List<Car> allCars = InitializeCars(noOfCars,assignedRides);
            List<Ride> allRides = InitializeRides(rides,bonus,totalSteps);
            List<int> activeSteps = new List<int>();
            
            while (currentStep < totalSteps && currentStep!=-1 && allRides.Count>0) {
                foreach(Car currentCar in allCars) {
                    if (currentCar.IsFreeAt(currentStep)) {
                        int bestScore = int.MaxValue;
                        Ride bestRide = null;
                        foreach(Ride ride in allRides) {
                            int score = ride.ScoreRide(currentCar, currentStep);
                            if(score < bestScore) {
                                bestScore = score;
                                bestRide = ride;
                            }
                        }
                        currentCar.TakeRide(bestRide, currentStep);
                        assignedRides[currentCar.Index].Add(bestRide.Index);
                        allRides.Remove(bestRide);
                        if (allRides.Count == 0) {
                            break;
                        }
                        InsertSorted(activeSteps, currentCar.ActiveTill);
                    }
                }
                currentStep = NextActiveStep(activeSteps);
            }
            return assignedRides;


            //return new List<List<int>> {
            //    new List<int> {
            //        0
            //    },
            //    new List<int> {
            //        2,1
            //    }
            //};
        }

        public static int GetDist(Tuple<int, int> start, Tuple<int, int> end) {
            return Math.Abs(start.Item1 - end.Item1) + Math.Abs(start.Item2 - end.Item2);
        }

        static int NextActiveStep(List<int> array) {
            if (array.Count == 0) {
                return -1;
            }
            int top = array[0];
            while(array.Count>0&&array[0] == top) {
                array.RemoveAt(0);
            }
            return top;
        }

        static void InsertSorted(List<int> array, int value) {
            // TEST
            for(int i = 0; i < array.Count; i++) {
                if (array[i] >= value) {
                    array.Insert(i, value);
                    return;
                }
            }
            array.Add(value);
        }

        static List<Ride> InitializeRides(List<List<int>> rides,int bonus, int totalSteps) {
            Ride.bonus = bonus;
            Ride.totalSteps = totalSteps;
            // Readonly

            List<Ride> allRides = new List<Ride>();
            int index = 0;
            foreach (List<int> ride in rides) {
                Ride rideX = new Ride(ride, index++);
                allRides.Add(rideX);
            }
            return allRides;
        }

        static List<Car> InitializeCars(int noOfCars, List<List<int>> assignedRides) {
            List<Car> cars = new List<Car>();
            for(int i = 0; i < noOfCars; i++) {
                cars.Add(new Car(i));
                assignedRides.Add(new List<int>());
            }
            return cars;
        }
    }
}
