﻿using System.Collections.Generic;
using System.Linq;

namespace NeuroWeb.EXMPL.OBJECTS {
    public class Tensor {
        public Tensor(Matrix matrix) => Body = new List<Matrix> { matrix };
        
        public Tensor(List<Matrix> matrix) => Body = matrix;
        
        public List<Matrix> Body { get; set; }

        public List<double> GetValues() {
            var a = new List<double>();
            return Body.Aggregate(a, (current, matrix) => (List<double>)current.Concat(matrix.GetAsList()));
        }
    }

    public class Filter : Tensor {
        public Filter(Matrix matrix) : base(matrix) {
            Body = new List<Matrix> { matrix };
        }

        public Filter(List<Matrix> matrix) : base(matrix) {
            Body = matrix;
        }
    }
}