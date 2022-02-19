using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImageDigger
{
    public class Dates
    {
        public string date { get; set; }
        public List<Rootobject> Images { get; set; }
        
    }

    
public class Rootobject
{
public string identifier { get; set; }
public string caption { get; set; }
public string image { get; set; }
public string version { get; set; }
public Centroid_Coordinates centroid_coordinates { get; set; }
public Dscovr_J2000_Position dscovr_j2000_position { get; set; }
public Lunar_J2000_Position lunar_j2000_position { get; set; }
public Sun_J2000_Position sun_j2000_position { get; set; }
public Attitude_Quaternions attitude_quaternions { get; set; }
public string date { get; set; }
public Coords coords { get; set; }
}

public class Centroid_Coordinates
{
public float lat { get; set; }
public float lon { get; set; }
}

public class Dscovr_J2000_Position
{
public float x { get; set; }
public float y { get; set; }
public float z { get; set; }
}

public class Lunar_J2000_Position
{
public float x { get; set; }
public float y { get; set; }
public float z { get; set; }
}

public class Sun_J2000_Position
{
public float x { get; set; }
public float y { get; set; }
public float z { get; set; }
}

public class Attitude_Quaternions
{
public float q0 { get; set; }
public float q1 { get; set; }
public float q2 { get; set; }
public float q3 { get; set; }
}

public class Coords
{
public Centroid_Coordinates1 centroid_coordinates { get; set; }
public Dscovr_J2000_Position1 dscovr_j2000_position { get; set; }
public Lunar_J2000_Position1 lunar_j2000_position { get; set; }
public Sun_J2000_Position1 sun_j2000_position { get; set; }
public Attitude_Quaternions1 attitude_quaternions { get; set; }
}

public class Centroid_Coordinates1
{
public float lat { get; set; }
public float lon { get; set; }
}

public class Dscovr_J2000_Position1
{
public float x { get; set; }
public float y { get; set; }
public float z { get; set; }
}

public class Lunar_J2000_Position1
{
public float x { get; set; }
public float y { get; set; }
public float z { get; set; }
}

public class Sun_J2000_Position1
{
public float x { get; set; }
public float y { get; set; }
public float z { get; set; }
}

public class Attitude_Quaternions1
{
public float q0 { get; set; }
public float q1 { get; set; }
public float q2 { get; set; }
public float q3 { get; set; }
}





}
