﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

/// <summary>
/// For clarification, these arrays won't work like Lua's ones.
///
/// No dynamic resizing by indexing.
///
/// 0 to Length-1 indexed
///
/// you can access like python.
///
/// example: arr[-2] returns the penultimate element
///
/// you can do arr[0 .. 3] and it will return the first 3 elements
///
/// you cannot convert it into a table just like that
/// </summary>
public class ArrayObject : Object
{
   public const string Name = "Array";
   public List<Object> array = new List<Object>();

   public override string Type()
   {
      return Name;
   }

   public ArrayObject(List<Object> arr)
   {
      array = arr;
   }
   
   public ArrayObject()
   {
      
   }


   public Object Append(Object obj)
   {
      array.Add(obj);
      return obj;
   }

   public Object Pop()
   {
      if (array.Count == 0) return Null.NULL;
      Object last = array[array.Count - 1];
      array.RemoveAt(array.Count - 1);
      return last;
   }

   public Object RemoveAt(int idx)
   {
      if (idx >= array.Count) return Null.NULL;
      Object last = array[idx];
      array.RemoveAt(idx);
      return last;
   }

   public Object Shift()
   {
      return RemoveAt(0);
   }

   public Object Slice(int start, int end)
   {
      if (end > array.Count)
      {
         return new Error($"slice operation is out of bounds: end {end} and length {array.Count}");
      }

      if (start > end)
      {
         return new Error($"start {start} cannot be bigger than end {end}");
      }

      if (start < 0 || start >= array.Count)
      {
         return new Error($"slice operation is out of bounds: start {start} and length {array.Count}");
      }

      List<Object> newList = array;
      for (int i = 0; i < newList.Count; i++)
      {
         newList[i] = newList[i].Clone();
      }
      
      return new ArrayObject(newList.GetRange(start, end - start));
   }

   public Object Get(int idx)
   {
      if (idx < 0)
      {
         idx = array.Count + idx;
      }

      if (idx < 0 || idx >= array.Count)
      {
         return new Error($"out of bounds, length: {array.Count} ; idx: {idx}");
      }

      return array[idx];
   }

   public Object Set(int idx, Object set)
   {
      if (idx >= array.Count)
      {
         return new Error($"out of bounds, length: {array.Count} ; idx: {idx}");
      }
      array[idx] = set;
      return Null.NULL;
   }

   public ArrayObject Concatenate(ArrayObject arrayObj)
   {
      List<Object> objs = new List<Object>(array.Count + arrayObj.array.Count);
      array.ForEach(el =>
      {
         objs.Add(el.Clone());
      });
      arrayObj.array.ForEach(el =>
      {
         objs.Add(el.Clone());
      });
      return new ArrayObject(objs);
   }


   public override string ToString()
   {
      string print = "{";
      for (int i = 0; i < array.Count; ++i)
      {
         string space = i >= 1 ? ", " : "";
         print += $"{space}{array[i]}";
      }
      print += "}";
      return print;
   }


   public override Object FromJson(JToken token)
   {
      var arrayToken = token.ToArray();
      Object[] arrayObjects = new Object[arrayToken.Length];
      int i = 0;
      foreach (var tokenElement in arrayToken)
      {
         arrayObjects[i] = ObjectJSON.ParseJsonToken(tokenElement);
         i++;
      }
      array = arrayObjects.ToList();
      return this;
   }

   public override bool EqualDeep(Object target)
   {
      if (!(target is ArrayObject arrayObj)) return false;
      for (int i = 0; i < arrayObj.array.Count; ++i)
      {
         if (!arrayObj.array[i].EqualDeep(array[i]))
         {
            return false;
         }
      }
      return true;
   }
}
