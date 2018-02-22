/*  
    Copyright (C) 2017 G. Michael Barnes
 
    The file NavNode.cs is part of AGMGSKv8 a port and update of AGXNASKv7 from
    MonoGames 3.4 to MonoGames 3.5  

    AGMGSKv8 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

/*
 * Project 2
 * Comp 565 Spring 2017
 */


#region Using Statements
using System;
using System.IO;  // needed for trace()'s fout
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace AGMGSKv8
{

	/// <summary>
	/// A WayPoint or Marker to be used in path following or path finding.
	/// Five types of WAYPOINT:
	/// <list type="number"> VERTEX, a non-navigatable terrain vertex </list>
	/// <list type="number"> WAYPOINT, a navigatable terrain vertex </list>
	/// <list type="number"> PATH, a node in a path (could be the result of A*) </list>
	/// <list type="number"> OPEN, a possible node to follow in an A*path</list>
	/// <list type="number"> CLOSED, a node that has been evaluated by A* </list>

	/// 
	/// 2/14/2012  last update
	/// </summary>
	public class NavNode : IComparable<NavNode>
	{
		public enum NavNodeEnum { VERTEX, WAYPOINT, PATH, OPEN, CLOSED };
		private List<NavNode> adjacentNodes = new List<NavNode>();
		private Vector3 translation;
		private NavNodeEnum navigatable;
		private Vector3 nodeColor;
		private NavNode prevNode;
        private double goalCost;
        private double sourceCost;
		private double totalCost;

		public NavNode PrevNode
		{
			get { return prevNode; }
			set { prevNode = value; }
		}

		public List<NavNode> AdjacentNodes
		{
			get { return adjacentNodes; }
		}

		private double GoalCost(NavNode goal)
        {
			if (prevNode == null)
				sourceCost = 0;
			else if (goalCost < 0)
				goalCost = DistanceBetween(goal);
            return goalCost;
        }

        private double SourceCost()
        {
			if (prevNode == null)
				sourceCost = 0;
			else if (sourceCost < 0)
				sourceCost = PrevNode.SourceCost() + 1;
			return sourceCost;
        }

        public double TotalCost(NavNode goal)
        {
			if (totalCost < 0)
				totalCost = GoalCost(goal) + SourceCost();
			return totalCost;
        }

        public double DistanceBetween(NavNode n)
        {
            double x1 = (double)Translation.X / 150;
            double z1 = (double)translation.Z / 150;

			double x2 = (double)n.translation.X / 150;
			double z2 = (double)n.translation.Z / 150;

            double value = Math.Pow((x2 - x1), 2) + Math.Pow((z2 - z1), 2);

            return Math.Sqrt(value);
        }

        public NavNode(Vector3 pos)
		{
			translation = pos;
			Navigatable = NavNodeEnum.VERTEX;
			prevNode = null;
			goalCost = sourceCost = totalCost = -1;
		}

		public NavNode(Vector3 pos, NavNodeEnum nType)
		{
			translation = pos;
			Navigatable = nType;
			prevNode = null;
			goalCost = sourceCost = totalCost = -1;
		}

		// properties

		public Vector3 NodeColor
		{
			get { return nodeColor; }
		}

		/// <summary>
		/// When changing the Navigatable type the WAYPOINT's nodeColor is 
		/// also updated.
		/// </summary>
		public NavNodeEnum Navigatable
		{
			get { return navigatable; }
			set
			{
				navigatable = value;
				switch (navigatable)
				{
					case NavNodeEnum.VERTEX: nodeColor = Color.Black.ToVector3(); break;  // black
					case NavNodeEnum.WAYPOINT: nodeColor = Color.Yellow.ToVector3(); break;  // yellow
					case NavNodeEnum.PATH: nodeColor = Color.Blue.ToVector3(); break;  // blue
					case NavNodeEnum.OPEN: nodeColor = Color.White.ToVector3(); break;  // white
					case NavNodeEnum.CLOSED: nodeColor = Color.Red.ToVector3(); break;  // red
                }
            }
		}

		public Vector3 Translation
		{
			get { return translation; }
		}

		public int CompareTo(NavNode n)
		{
			if (totalCost < n.totalCost) return -1;
			else if (totalCost > n.totalCost) return 1;
			else return 0;
		}

	}
}
