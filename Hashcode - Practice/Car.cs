using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hashcode___Practice {
    public class Car {
        public int Index { get; }
        public Tuple<int, int> Location { get { return _location; } }
        Tuple<int, int> _location;
        public int ActiveTill { get { return _activeTill;  }  }
        int _activeTill;

        public Car(int ind) {
            Index = ind;
            _location = new Tuple<int, int>(0, 0);
            _activeTill = -1;
        }

        public bool IsFreeAt(int currentStep) {
            return currentStep >= ActiveTill;
        }

        public void TakeRide(Ride ride, int currentStep) {
            int carArrives = SelfDriving.GetDist(Location, ride.StartLocation) + currentStep;
            int rideStart = (carArrives <= ride.EarliestStart)? ride.EarliestStart: rideStart = carArrives;

            _activeTill = rideStart + ride.Raw_score;
            _location = ride.EndLocation;

        }
    }
}
