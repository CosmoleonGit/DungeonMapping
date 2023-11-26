using System.Collections.Generic;

/*
 * This implementation of the AStar pathfinding algorithm in MonoGame is from
 * https://github.com/prime31/Nez/blob/master/Nez.Portable/AI/Pathfinding/AStar/
 */
public interface IAstarGraph<T>
{
    /// <summary>
    /// The getNeighbors method should return any neighbor nodes that can be reached from the passed in node
    /// </summary>
    /// <returns>The neighbors.</returns>
    /// <param name="node">Node.</param>
    IEnumerable<T> GetNeighbors(T node);

    /// <summary>
    /// calculates the cost to get from 'from' to 'to'
    /// </summary>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    int Cost(T from, T to);

    /// <summary>
    /// calculates the heuristic (estimate) to get from 'node' to 'goal'. See WeightedGridGraph for the common Manhatten method.
    /// </summary>
    /// <param name="node">Node.</param>
    /// <param name="goal">Goal.</param>
    int Heuristic(T node, T goal);
}