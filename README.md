# Tellurian.Trains.Models.Planning
This package defines a model for planning timetables for model railways. The model contains the following objects:
* Stations and station tracks.
* Track stretches and their combination into timetable stretches.
* Trains with times and notes.
* Vehicle schedule for loco and trainset.

It also implements a set of validation rules to check conflicts.
The following checks are implemented:
* Station track comflict: when more that one train is scheduled to occupie the same track at overlapping time.
* Strech conflict: when more trains than available parallel tracks is scheduled between adjacent station in overlapping time.
* Train routes conflict: jumping over stations; all passed stations must have time.
* Train timing conflict: times in wrong order compared to route.
* To slow or to fast scheduling times.

Other checks that might be included are:
* That a loco is assigned to all parts of a train.
* Warn if train has more than one loco for some part (however this can be intentional when running multitraktion).
* That all loco parts have been assigned to a duty (except for stretches when secondary units don't require a driver).
