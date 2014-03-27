namespace OdtWriter
{
   public class OdtData
    {
        public string Name;
      
    }

   public class OdtDataSimple : OdtData
   {
       public string Data;
   }

   public class OdtDataArray : OdtData
   {
       public string[] Data;
   }

}
