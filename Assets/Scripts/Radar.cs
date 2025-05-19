using NUnit.Framework;
using System;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections.Generic;


public class Radar
{
    private Transform rootTransform;
    public Radar(Transform root, Collider myCollider)
    {
        rootTransform = root;
    }

    /// <summary>
    /// takes in a list of Touples where
    /// first input represents the layer to query
    /// second input is the max colliders to find
    /// third is the max detection distance around the querying object
    /// 
    /// returns list of list of found objects positions in a layer within the same order layers were provided in the query
    /// </summary>
  
    
}
