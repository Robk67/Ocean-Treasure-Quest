/*
 * Project 2
 * Comp 565 Spring 2017
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;


namespace AGMGSKv8

{
	public class NavGraph
	{
		public Dictionary<string, NavNode> graph; // key: "x:z"
		private List<NavNode> path = new List<NavNode>();
		private List<NavNode> open = new List<NavNode>();
		private List<NavNode> closed = new List<NavNode>();
		private List<NavNode> waypoint = new List<NavNode>();
		private NavNode source = null;
		private NavNode goal = null;

		public NavGraph()
		{
			graph = new Dictionary<string, NavNode>();
		}

		public void Reset()
		{
			path = new List<NavNode>();
			open = new List<NavNode>();
			closed = new List<NavNode>();
			waypoint = new List<NavNode>();
			setAll(NavNode.NavNodeEnum.WAYPOINT);
		}

		private void setAll(NavNode.NavNodeEnum nodeType)
		{
			foreach (KeyValuePair<String, NavNode> item in graph)
			{
				if (item.Value.Navigatable != NavNode.NavNodeEnum.VERTEX)
					item.Value.Navigatable = nodeType;
			}
		}

		private bool aStarPath()
		{
			source.Navigatable = NavNode.NavNodeEnum.OPEN;
			source.TotalCost(goal);
			open.Add(source);

			while (open.Count > 0)
			{
				open.Sort();
				NavNode curNode = open[0];
				if (curNode.DistanceBetween(this.goal) <= 1)
				{
					this.BuildPath(curNode);
					return true;
				}
				else
				{
					curNode.Navigatable = NavNode.NavNodeEnum.CLOSED;
					open.Remove(curNode);
					closed.Add(curNode);
					foreach (NavNode n in curNode.AdjacentNodes)
					{
						if (n.Navigatable != NavNode.NavNodeEnum.OPEN && n.Navigatable != NavNode.NavNodeEnum.CLOSED)
						{
							n.PrevNode = curNode;
							n.TotalCost(goal);
							n.Navigatable = NavNode.NavNodeEnum.OPEN;
							open.Add(n);
						}
					}

				}
			}
			return false;
		}

		public void Add(int x, int z)
		{
			if (!graph.ContainsKey(skey(x, z)))
				graph.Add(skey(x, z), new NavNode(new Vector3(x, 0, z)));
		}

		private String skey(int x, int z)
		{
			return String.Format("{0}::{1}", x, z);
		}

		private void BuildPath(NavNode n)
		{
			n.Navigatable = NavNode.NavNodeEnum.PATH;
			path.Add(n);
			while (n.PrevNode != null)
			{
				n = n.PrevNode;
				n.Navigatable = NavNode.NavNodeEnum.PATH;
				path.Add(n);
			}
			path.Reverse();
			return;
		}

		public NavNode this[int x, int z]
		{
			get { return graph[(skey(x, z))]; }
			set { graph[skey(x, z)] = value; }
		}
	}
}
