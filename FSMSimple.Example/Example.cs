using System;

namespace FSMSimple.Example
{
    class Example
    {
        public enum State
        {
            Unknown,
            Started,
            Working
        }

        public enum Event
        {
            StartEngine,
            StopEngine,
            TurnOnLights,
            TurnOffLights,
            Drive100km,
            AddFuel,
        }

        StateMachine car = new StateMachine();
        private bool _areLightsTurnedOn;
        private int _fuel = 40;
        private int _carTraveled = 0;
        public Example()
        {
            car.BeforeStateChanged += Car_BeforeStateChanged;
            car.AfterStateChanged += Car_AfterStateChanged;


            // Car can only start if it is not already started and if there is fuel
            AddTransition(State.Unknown, Event.StartEngine, IsThereEnoughFuel, ActionStartingEngine, State.Started, ActionEngineStarted);

            // Car can stop only if it is started
            AddTransition(State.Started, Event.StopEngine, null, null, State.Unknown, ActionPoweredOff);

            // Can turn on lights only if car is started and if lights are turned off
            AddTransition(State.Started, Event.TurnOnLights, AreLightsTurnedOff, null, State.Started, ActionTurnOnLights);

            // Can turn off lights only if car is started and if lights are turned on
            AddTransition(State.Started, Event.TurnOffLights, AreLightsTurnedOn, null, State.Started, ActionTurnOffLights);

            // Car can drive 100 km only if car is started and thre is more than 10 liters of fuel in the tank.
            AddTransition(State.Started, Event.Drive100km, IsThereEnoughFuel, null, State.Started, ActionDrive100km);

            // If there is no more fuel, engine will turn off
            AddTransition(State.Started, Event.Drive100km, null, null, State.Unknown, ActionCarStopped);

            // Fuel can be added to car only if car is turned off. Adding fuel will not start cart.
            AddTransition(State.Unknown, Event.AddFuel, null, null, State.Unknown, ActionAddFuel);
        }

        public void Show()
        {
            ProcessEvent(Event.StartEngine);
            ProcessEvent(Event.StopEngine);
            ProcessEvent(Event.StopEngine);
            ProcessEvent(Event.StopEngine);
            ProcessEvent(Event.StartEngine);
            ProcessEvent(Event.StartEngine);
            ProcessEvent(Event.TurnOnLights);
            ProcessEvent(Event.TurnOnLights);

            ProcessEvent(Event.Drive100km);
            ProcessEvent(Event.Drive100km);
            ProcessEvent(Event.Drive100km);
            ProcessEvent(Event.Drive100km);
            ProcessEvent(Event.Drive100km);

            ProcessEvent(Event.AddFuel);
            ProcessEvent(Event.Drive100km);

            ProcessEvent(Event.StartEngine);
            ProcessEvent(Event.Drive100km);
            ProcessEvent(Event.StopEngine);
        }

        private void AddTransition(State startState, Event eventCode, Func<bool> condition, Action<StateMachineArgs> preAction, State endState, Action<StateMachineArgs> postAction)
        {
            car.AddTransition((int)startState, (int)eventCode, condition, preAction, (int)endState, postAction);
        }

        private void ProcessEvent(Event eventCode)
        {
            Write($"Firing event: {eventCode} ");
            car.ProcessEvent((int)eventCode);
            Write("--------------------------");
        }

        private void Car_BeforeStateChanged(StateMachineArgs obj)
        {
            Console.WriteLine($"State is going to change: {(State)obj.StartState} >> {(State)obj.EndState}");
        }

        private void Car_AfterStateChanged(StateMachineArgs obj)
        {
            Console.WriteLine($"State changed: {(State)obj.StartState} >> {(State)obj.EndState}");
        }

        private void ActionStartingEngine(StateMachineArgs obj)
        {
            Write("Starting engine...");
        }

        private void ActionEngineStarted(StateMachineArgs obj)
        {
            Write("Engine is started");
        }

        private void ActionPoweredOff(StateMachineArgs obj)
        {
            Write("Engine is turned off");
        }

        private bool AreLightsTurnedOn()
        {
            if (!_areLightsTurnedOn)
                Write("STOP! Lights must be turned on before turning them off");

            return _areLightsTurnedOn;
        }

        private bool AreLightsTurnedOff()
        {
            if (_areLightsTurnedOn)
                Write("STOP! Lights must be turned off before turning them on");

            return !_areLightsTurnedOn;
        }

        private void ActionTurnOnLights(StateMachineArgs obj)
        {
            Write("Lights are turned on now!");
            _areLightsTurnedOn = true;
        }

        private void ActionTurnOffLights(StateMachineArgs obj)
        {
            Write("Lights are turned off now!");


            _areLightsTurnedOn = false;
        }

        private void ActionDrive100km(StateMachineArgs obj)
        {
            _fuel -= 10;

            if (_fuel < 0)
                _fuel = 0;

            _carTraveled += 100;

            Write($"Car drove 100 km. Remaining fuel: {_fuel}. Traveled: {_carTraveled}");
        }

        private bool IsThereEnoughFuel()
        {
            if (_fuel >= 10)
                return true;

            Write($"Stop! There is no enough fuel: {_fuel}");
            return false;
        }

        private void ActionCarStopped(StateMachineArgs obj)
        {
            Write($"Car stopped due to lack of fuel. Traveled: {_carTraveled}");
        }

        private void ActionAddFuel(StateMachineArgs obj)
        {
            _fuel += 50;
            Write($"Fueld added. Fuel: {_fuel}");
        }

        private void Write(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}

