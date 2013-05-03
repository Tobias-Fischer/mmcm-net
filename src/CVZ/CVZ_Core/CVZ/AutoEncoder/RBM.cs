using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVZ_Core.CVZ.AuoEncoder
{
    /// <summary>
    /// A restricted boltzman machine implementation
    /// </summary>
    class RBM
    {
	   static Random m_rand = new Random();

	   public float m_learningRate;

	   public int m_hiddenLayerSize;

	   public int m_inputLayerSize;

	   Matrix m_weights;

	   Matrix m_a;

	   Matrix m_b;

	   public RBM(int inputSize, int hiddenSize)
	   {
		  m_inputLayerSize = inputSize;
		  m_hiddenLayerSize = hiddenSize;
		  m_a = new Matrix(1,m_inputLayerSize); //To do : check in the algorithm
		  m_b = new Matrix(1,m_hiddenLayerSize);
		  m_weights = new Matrix(m_inputLayerSize, m_hiddenLayerSize);
	   }

	   void Train(Matrix input)
	   {
		  Matrix hiddenUnitP = ComputeProbabilitiesH(input);
		  Matrix sampleHidden = Sample(hiddenUnitP);
		  Matrix inputUnitP = ComputeProbabilitiesI(sampleHidden);
		  Matrix sampleInput = Sample(inputUnitP);
		  Matrix positiveGradient = Matrix.outerProduct(input, sampleHidden);
		  Matrix hiddenUnitP2 = ComputeProbabilitiesH(sampleInput);
		  Matrix sampleHidden2 = Sample(hiddenUnitP2);
		  Matrix negativeGradient = Matrix.outerProduct(sampleInput, sampleHidden2);
        
		  //Adapt weights
		  m_weights = m_weights + m_learningRate * (positiveGradient - negativeGradient);
	   }

    Matrix ComputeProbabilitiesH(Matrix input)
    {
        Matrix hiddenUnit = new Matrix(1,m_hiddenLayerSize);
        for(int i=0; i<m_inputLayerSize; i++)
        {
            for(int j=0; j<m_hiddenLayerSize; j++)
            {
                hiddenUnit[0,j] +=  m_weights[i,j] * input[0,i];
            }
        }

        for(int j=0; j<m_hiddenLayerSize; j++)
        {
                hiddenUnit[0,j] = 1.0f/(float)(1+Math.Exp(m_b[0,j] + hiddenUnit[0,j] ));
        }
        return hiddenUnit;
    }
	   

    Matrix ComputeProbabilitiesI(Matrix hiddenSample)
    {
        Matrix inputUnit = new Matrix(1,m_inputLayerSize);
        for(int i=0; i<m_hiddenLayerSize; i++)
        {
            for(int j=0; j<m_inputLayerSize; j++)
            {
                inputUnit[0,j] +=  m_weights[j,i] * hiddenSample[0,i];
            }
        }

        for(int j=0; j<m_inputLayerSize; j++)
        {
                inputUnit[0,j] = 1.0f/(float)(1+Math.Exp(m_a[0,j] + inputUnit[0,j] ));
        }
        return inputUnit;
    }
    
    Matrix Sample(Matrix hiddenUnit)
    {
        Matrix sample = new Matrix(1,m_hiddenLayerSize);
        for(int j=0; j<m_hiddenLayerSize; j++)
        {
		  if (hiddenUnit[0,j] > (float)m_rand.NextDouble())
			 sample[0,j] = 1.0f;
		  else
			 sample[0, j] = 0.0f;
        }
        return sample;
    }

    }
}
