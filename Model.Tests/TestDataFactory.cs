using System.Collections.Generic;
using System.Linq;

namespace Tellurian.Trains.Models.Planning.Tests
{
    internal static class TestDataFactory
    {
        public static StationTrack CreateStationTrack()
        {
            var result = StationTrack.Example;
            result.Station = Station.Example;
            return result;
        }

        public static StationExit CreateStationExit()
        {
            var result = StationExit.Example;
            result.Station = Station.Example;
            return result;
        }

        public static StationExit CreateOtherStationExit(string name)
        {
            return new StationExit(name) { Station = new Station("Somwhere else", "Swe") };
        }

        public static void Init()
        {
            Stations = new[] { CreateStation1(), CreateStation2(), CreateStation3() };
        }

        public static IEnumerable<Station> Stations;

        internal static Station CreateStation1()
        {
            var station = new Station("Göteborg", "G");
            station.Add(new StationExit("Vänster"));
            station.Add(new StationTrack("1"));
            station.Add(new StationTrack("2"));
            station.Add(new StationTrack("3"));
            station.Add(new StationTrack("4"));
            return station;
        }

        private static Station CreateStation2()
        {
            var station = new Station("Ytterby", "Yb");
            station.Add(new StationExit("Vänster"));
            station.Add(new StationExit("Höger"));
            station.Add(new StationTrack("1"));
            station.Add(new StationTrack("2"));
            return station;
        }

        private static Station CreateStation3()
        {
            var station = new Station("Stenungsund", "Snu");
            station.Add(new StationExit("Vänster"));
            station.Add(new StationExit("Höger"));
            station.Add(new StationTrack("1"));
            station.Add(new StationTrack("2"));
            return station;
        }

        public static IEnumerable<Train> CreateTrains(string category, Time startTime)
        {
            return new[] {
                CreateTrainInForwardDirection(category, "1", startTime)
            };
        }

        public static Train CreateTrainInForwardDirection(string category, string number, Time startTime)
        {
            var stations = Stations.ToArray();
            var train = new Train(number) { Category = category };
            train.Add(new StationCall(stations[0]["3"], startTime, startTime));
            train.Add(new StationCall(stations[1]["2"], startTime.Add(25), startTime.Add(30)));
            train.Add(new StationCall(stations[2]["1"], startTime.Add(55), startTime.Add(55)));
            return train;
        }

        public static Train CreateTrainInOppositeDirection(string category, string number, Time startTime)
        {
            var stations = Stations.ToArray();
            var train = new Train(number) { Category = category };
            train.Add(new StationCall(stations[2]["2"], startTime, startTime));
            train.Add(new StationCall(stations[1]["1"], startTime.Add(25), startTime.Add(30)));
            train.Add(new StationCall(stations[0]["3"], startTime.Add(55), startTime.Add(55)));
            return train;
        }

        public static Train CreateTrain1()
        {
            return CreateTrainInForwardDirection("Godståg", "1234", new Time(12, 00));
        }

        public static Train CreateTrain2()
        {
            return CreateTrainInOppositeDirection("Persontåg", "4321", new Time(12, 00));
        }

        public static Timetable CreateTimetable()
        {
            var timetable = new Timetable("Test", Layout());
            timetable.AddTrain(CreateTrain1());
            timetable.AddTrain(CreateTrain2());
            return timetable;
        }

        public static TrackLayout Layout()
        {
            var layout = new TrackLayout("Test");
            foreach (var s in Stations) layout.Add(s);
            var timetableStretch = new TimetableStretch("1");
            var ss = Stations.ToArray();
            timetableStretch.Add(layout.Add(ss[0].Exit("Vänster"), ss[1].Exit("Höger"), 10));
            timetableStretch.Add(layout.Add(ss[1].Exit("Vänster"), ss[2].Exit("Höger"), 10));
            layout.Add(timetableStretch);
            return layout;
        }
    }
}
