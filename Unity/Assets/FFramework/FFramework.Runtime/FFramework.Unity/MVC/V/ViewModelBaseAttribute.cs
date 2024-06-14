using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public class ViewModelBaseAttribute : Attribute
{
}


[AttributeUsage(AttributeTargets.GenericParameter,AllowMultiple =false,Inherited =false)]
public class ViewModelParameterAttribute : Attribute
{

}