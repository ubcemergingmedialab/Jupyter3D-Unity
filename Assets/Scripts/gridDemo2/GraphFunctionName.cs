/* Created by Rayhan Fakim, this script is used to have a menu in the inspector 
 * in order to change the function right from Unity. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// list of functions that are available in the Inpector of the GameObject

public enum GraphFunctionName
{
    // The first part which is composed of a letters is the name of the function that
    // will be found in the menu of the inspector. The second part which is composed of
    // a number is used to reference which function is used in VR. As we change to next
    // function in VR, the number is incremented and thus goes to the next function 
    Sine = 0,
    Sine2DFunction1 = 1,
    Sine2DFunction2 = 2,
    MultiSine = 3,
    MultiSine2DFunction = 4,
    MexicanHat = 5, 
    Gauss = 6
}
