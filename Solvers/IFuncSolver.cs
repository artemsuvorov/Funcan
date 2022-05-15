using System;
using Funcan.Domain;

namespace Funcan.Solvers
{
    public interface IFuncSolver
    {
        PointSet Solve(Func<double, double> function);
    }
}