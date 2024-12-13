Polygon/Bezier Curve Editor 
**Introduction**
This repository contains the implementation of a polygon/bezier curve editor, designed to facilitate the creation, editing, and management of polygonal and Bezier curves in a 2D space. The editor focuses on intuitive operations and supports constraints, such as horizontal, vertical edges, and specific segment lengths, while maintaining G0, G1, and C1 continuity.

The editor has the following key features:

Polygon Operations:
Adding a new polygon.
Deleting a polygon.
Editing (moving vertices, control points).

Editing Operations:
Moving a vertex or Bezier control point.
Deleting a vertex.
Adding a vertex in the middle of an edge.
Moving the entire polygon.

Edge Constraints:
Horizontal, vertical constraints.
Fixed edge length.
Maximum one constraint per edge.
Two adjacent edges cannot both be horizontal or both vertical.

Visibility of Constraints:
Constraints are displayed as icons on the corresponding edge.
Constraints can be removed or modified.

Bezier Segments:
Edges can be converted into cubic Bezier segments.
Control points are displayed for Bezier curves.
Switching between linear and Bezier representation is possible.

Continuity Constraints for Bezier Segments:
G0 (vertex continuity).
G1 (tangential continuity).
C1 (tangent vector continuity).
Switching between G1 (A-K) and C1 (L-Z) continuity based on the naming convention.

Drawing Algorithms:
Line segments using the Bresenham algorithm.
Bezier segments with an incremental algorithm.
