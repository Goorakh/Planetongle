using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InputManager : Singleton<InputManager>
{
    public InputData[] DefaultInputs;

    Dictionary<ActionType, InputData> _inputs;

    protected override void Awake()
    {
        base.Awake();

        _inputs = new Dictionary<ActionType, InputData>();
        foreach (InputData input in DefaultInputs)
        {
            _inputs.Add(input.Action, input);
        }
    }

    public bool IsInputActive(ActionType actionType)
    {
        if (_inputs.TryGetValue(actionType, out InputData inputData))
            return inputData.IsActive();

        throw new ArgumentException("Action \"" + actionType + "\" does not exist in the inputs dictionary");
    }
}
