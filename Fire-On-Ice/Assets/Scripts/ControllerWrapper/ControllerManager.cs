﻿using UnityEngine;
using System.Collections;

public class ControllerManager  {
   
    public enum ControlType { Xbox, PS3, PS4 };
    public enum OperatingSystem { Win, OSX, Linux };
    public static OperatingSystem currentOS;

    public static KeyboardWrapper keyboardWrapper = new KeyboardWrapper();

    public static ControllerInputWrapper[] playerControls = new ControllerInputWrapper[4];

    public ControllerManager()
    {
        setUpControls();
    }

    public static void setUpControls()
    {
        setUpPlatform();
        string[] controllerNames = Input.GetJoystickNames();
        //Debug.Log("Controllers connected: " + controllerNames.Length);
        for (int i = 0; i < controllerNames.Length; i++)
        {
            playerControls[i] = getControllerType(i + 1);
        }
    }

    public static ControllerInputWrapper getControllerType(int joyNum)
    {
        //Debug.Log(joyNum);
        string[] controllerNames = Input.GetJoystickNames();
        if (joyNum == 0)
        {
            return new KeyboardWrapper();
        }
        if (joyNum < 0 || joyNum > controllerNames.Length)
        {
            return null;
        }
        joyNum--;
        string name = controllerNames[joyNum];

        //Debug.Log("Controllers connected: " + controllerNames.Length);

        if (name.Contains("Wireless"))
        {
            return new PS4ControllerWrapper();
        }
        else if (name.Contains("Logitech"))
        {
            return new LogitechControllerWrapper();
        }
        else
        {
            return new XboxControllerWrapper();
        }
           

    }

    static void setUpPlatform()
    {
		//Debug.Log ("platform: " + Application.platform);
		if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer 
			|| Application.platform == RuntimePlatform.OSXEditor)
        {
            currentOS = OperatingSystem.OSX;
        }
        else
        {
            currentOS = OperatingSystem.Win;
        }
    }

    public static float GetAxis(ControllerInputWrapper.Axis axis, int joyNum, bool isRaw = false)
    {
        joyNum--;//Off by one error
        if (joyNum < 0)
        {
            if (joyNum == -1)
            {
                return keyboardWrapper.GetAxis(axis, joyNum, isRaw);
            }
            return 0;
            
        }
        if (playerControls[joyNum] == null)
        {
            return 0;
        }
        return playerControls[joyNum].GetAxis(axis, joyNum + 1, isRaw);
    }

    public static float GetTrigger(ControllerInputWrapper.Triggers trigger, int joyNum, bool isRaw = false)
    {
        joyNum--;

        if (joyNum < 0)
        {
            if (joyNum == -1)
            {
                return keyboardWrapper.GetTrigger(trigger, joyNum, isRaw);
            }
            return 0;
        }
        if (playerControls[joyNum] == null)
        {
            return 0;
        }
		//Debug.Log (trigger);
        return playerControls[joyNum].GetTrigger(trigger, joyNum + 1, isRaw);
    }

    public static bool GetButton(ControllerInputWrapper.Buttons button, int joyNum, bool isDown = false)
    {
        joyNum--;
        //Debug.Log(joyNum);
        if (joyNum < 0)
        {
            if (joyNum == -1)
            {
                return keyboardWrapper.GetButton(button, joyNum, isDown);
            }
            return false;
            
        }
        if (playerControls[joyNum] == null)
        {
            return false;
        }
        return playerControls[joyNum].GetButton(button, joyNum + 1, isDown);
    }

    public static bool GetButtonUp(ControllerInputWrapper.Buttons button, int joyNum)
    {
        joyNum--;
        if (joyNum < 0 || playerControls[joyNum] == null)
        {
            return false;
        }
        return playerControls[joyNum].GetButtonUp(button, joyNum + 1);
    }

    public static bool GetButtonAll(ControllerInputWrapper.Buttons button, bool isDown)
    {
        //This definitely needs an update........... Like seriously..
        int i = 0;
        foreach (ControllerInputWrapper cW in playerControls)
        {
            if (!cW.GetButton(button, i, isDown)){
                return false;
            }
        }
        return true;
    }

    void setPlatform()
    {
		
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            currentOS = OperatingSystem.Win;
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            currentOS = OperatingSystem.OSX;
        }
    }
}
