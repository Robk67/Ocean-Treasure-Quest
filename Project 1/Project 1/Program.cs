/*
 * Program.cs is the starting point for AGMGSK applications.
 * 
 * For Windows users of the XNA 4 API (not MonoGames 3.4) 
 * you must select Project | Properties | XNA Game Studio
 * and select Game Profile:  "Use HiDef".  
 * Do this once at initial project configuration.
 * Project 2
 * Comp 565 Spring 2017
 */

using System;
// using System.Collections;  // needed any more 1/11/2017

namespace AGMGSKv8  {
static class Program {
   /// <summary>
   /// The main entry point for the application.
   /// </summary>
   static void Main(string[] args) {
      using (Stage stage = new Stage()) {
            stage.Run();
      }
   }
}
}

