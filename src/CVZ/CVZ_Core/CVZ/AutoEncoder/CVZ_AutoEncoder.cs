using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVZ_Core;

namespace CVZ_Core.CVZ.AuoEncoder
{
    class CVZ_AutoEncoder : IConvergenceZone
    {
	   List<RBM> layersIn;
	   List<RBM> layersOut;

	   public CVZ_AutoEncoder(string name, int[] layersSize):base(name)
	   {
		  int layerCount = layersSize.Count();
        
		  for(int i=1; i<=layerCount; i++)
		  {
			 layersIn.Add(new RBM(layersSize[i - 1], layersSize[i]));
		  }
	   }

	   protected override int hierarchicalModalityDesiredSize()
	   {
		  throw new NotImplementedException();
	   }

	   protected override float[] feedForward()
	   {
		  throw new NotImplementedException();
	   }

	   protected override void ComputePredictedValues()
	   {
		  base.ComputePredictedValues();
	   }

	   public override void AddModality(Modalities.IModality m, float influence)
	   {
		  base.AddModality(m, influence);
	   }

	   public override void RemoveModality(string modalityName)
	   {
		  base.RemoveModality(modalityName);
	   }

	   public void Train()
	   {

	   }
	   public void UnrollRBMs()
	   {
		  layersOut.Clear();
		  for(int i=layersIn.Count -1 ; i<=0; i++)
		  {
			 layersOut.Add(new RBM(layersIn[i].m_hiddenLayerSize, layersIn[i].m_inputLayerSize));
		  }
	   }
    }
}
