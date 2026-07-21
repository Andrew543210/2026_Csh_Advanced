using _2026_Csh_Advanced.sprint5_Collections;
using _2026_Csh_Advanced.sprint1_Classes;
using _2026_Csh_Advanced.sprint2_InhPol;
using _2026_Csh_Advanced.sprint10_Solid;
using _2026_Csh_Advanced.sprint8_TPL;
using _2026_Csh_Advanced.sprint11_Reflection;

class Program
{
    static async Task Main(string[] args)
    {
        #region sprint1
        //Classes.RunClasses();
        #endregion
        #region sprint2
        //Inheritance.RunInheritance();
        #endregion
        #region sprint5
       Collections.RunCollections();
        #endregion

        #region sprint8
        // await FlightAggregatorDemo.SimulateTicketSearchAsync();
        // CarWashDemo.RunSimulationAsync().GetAwaiter().GetResult();
        // TplSprint.RunTplSprint();
        #endregion
        
        #region sprint10
        //AppLogger.SolidDemo.RunSolid();
        #endregion

        #region Reflection
        //ReflectionDemo.RunReflectionSprint();
        #endregion
    }
}