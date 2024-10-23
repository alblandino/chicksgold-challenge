using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChallengeController : Controller
    {
        [HttpPost]
        public IActionResult Get([FromBody] Request request)
        {
            var challenge = new Challenge(request.x_capacity, request.y_capacity, request.z_amount_wanted);

            // Verificar si es posible resolver el problema
            if (!challenge.possible())
            {
                return StatusCode(500, new { solution = "No solution" }); // Retornar 500 en caso de no solución
            }

            // Ejecutar los métodos para obtener los pasos
            var stepsXToY = challenge.Pour(request.x_capacity, request.y_capacity, request.z_amount_wanted);
            var stepsYToX = challenge.Flush(request.x_capacity, request.y_capacity, request.z_amount_wanted);

            // Elegir la solución más corta
            var finalSteps = stepsXToY.Count < stepsYToX.Count ? stepsXToY : stepsYToX;

            var solutionList = new List<Response>();
            for (int i = 0; i < finalSteps.Count; i++)
            {
                var stepDetails = finalSteps[i];
                var (bucketX, bucketY) = ParseStep(stepDetails);
                
                solutionList.Add(new Response
                {
                    step = i + 1,
                    bucketX = bucketX,
                    bucketY = bucketY,
                    action = stepDetails,
                    status = i == finalSteps.Count - 1 ? "Solved" : null
                });
            }

            // Devolver la solución como un JSON con la estructura solicitada
            return Ok(new { solution = solutionList });
        }

        private (int, int) ParseStep(string step)
        {
            // Extraer los números de la descripción del paso
            var split = step.Split(new[] { "x:", "y:" }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length < 3)
            {
                return (0, 0);
            }

            try
            {
                int bucketX = int.Parse(split[1].Trim().Split(',')[0]);
                int bucketY = int.Parse(split[2].Trim().Split(',')[0]);
                return (bucketX, bucketY);
            }
            catch (FormatException)
            {
                return (0, 0);
            }
        }
    }

    public class Challenge
    {
        private int _xBucketSize;
        private int _yBucketSize;
        private int _targetAmount;

        public Challenge(int xBucketSize, int yBucketSize, int targetAmount)
        {
            _xBucketSize = xBucketSize;
            _yBucketSize = yBucketSize;
            _targetAmount = targetAmount;
        }

        public bool possible()
        {
            var maxBucket = Math.Max(_xBucketSize, _yBucketSize);
            return _targetAmount <= maxBucket && _targetAmount % GreatestCommonDivisor(_xBucketSize, _yBucketSize) == 0;
        }

        public List<string> Pour(int bucketX, int bucketY, int targetAmount)
        {
            var stepList = new List<string>();
            int fromBucketX = 0; // Jarra X vacía
            int toBucketY = 0;   // Jarra Y vacía

            stepList.Add($"Fill Bucket X - x:{fromBucketX = bucketX},y:{toBucketY}");

            while (fromBucketX != targetAmount && toBucketY != targetAmount)
            {
                var minPouredAmount = Math.Min(fromBucketX, bucketY - toBucketY);
                toBucketY += minPouredAmount;
                fromBucketX -= minPouredAmount;

                stepList.Add($"Transfer Bucket X to Bucket Y - x:{fromBucketX},y:{toBucketY}");

                // Verificar si se ha alcanzado el objetivo
                if (fromBucketX == targetAmount || toBucketY == targetAmount)
                {
                    break;
                }

                // Si la jarra Y está llena, la vaciamos
                if (toBucketY == bucketY)
                {
                    stepList.Add($"Empty Bucket Y - x:{fromBucketX},y:{toBucketY = 0}");
                    continue;
                }

                // Si la jarra X está vacía, la llenamos
                if (fromBucketX == 0)
                {
                    stepList.Add($"Fill Bucket X - x:{fromBucketX = bucketX},y:{toBucketY}");
                }
            }

            return stepList;
        }

        public List<string> Flush(int bucketX, int bucketY, int targetAmount)
        {
            var stepList = new List<string>();
            int fromBucketY = 0; // Jarra Y vacía
            int toBucketX = 0;   // Jarra X vacía

            stepList.Add($"Fill Bucket Y - x:{toBucketX},y:{fromBucketY = bucketY}");

            while (fromBucketY != targetAmount && toBucketX != targetAmount)
            {
                var minPouredAmount = Math.Min(fromBucketY, bucketX - toBucketX);
                toBucketX += minPouredAmount;
                fromBucketY -= minPouredAmount;

                stepList.Add($"Transfer Bucket Y to Bucket X - x:{toBucketX},y:{fromBucketY}");

                // Verificar si se ha alcanzado el objetivo
                if (fromBucketY == targetAmount || toBucketX == targetAmount)
                {
                    break;
                }

                // Si la jarra X está llena, la vaciamos
                if (toBucketX == bucketX)
                {
                    stepList.Add($"Empty Bucket X - x:{toBucketX = 0},y:{fromBucketY}");
                    continue;
                }

                // Si la jarra Y está vacía, la llenamos
                if (fromBucketY == 0)
                {
                    stepList.Add($"Fill Bucket Y - x:{toBucketX},y:{fromBucketY = bucketY}");
                }
            }

            return stepList;
        }

        private int GreatestCommonDivisor(int a, int b)
        {
            // Aseguramos que ambos números sean positivos
            a = Math.Abs(a);
            b = Math.Abs(b);
            // Continuar el proceso hasta que b se vuelva 0
            while (b != 0)
            {
                // Guardamos el valor de b en una variable temporal
                int temp = b;
                // El nuevo valor de b es el resto de a dividido por b
                b = a % b;
                // Asignamos a a el valor de temp (el antiguo b)
                a = temp;
            }
            // Cuando b es 0, a contiene el máximo común divisor
            return a;
        }

    }  
}
