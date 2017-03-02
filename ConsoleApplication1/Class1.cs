using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConsoleApplication1
{
    public class Path2D{}
    public class EnvironmentTile
    {

        // Assumptions:
        // All locations are origined at the parent objecte
        // The width of the overall road fragment is constant

        public RoadFragment Road;
        public List<LandSection> LeftSections;
        public List<LandSection> RightSections;
        public float Length;

        public float MaxSectionLength;
        public float MinSectionLength;


        public const float OVERALL_WIDTH = 100;


        public void GenerateRandom()
        {
            GenerateSections(false);
            GenerateSections(true);
        }

        private void GenerateSections(bool isRight)
        {
            // just need to generate the set of random fragments that add up to the overall length of the tile

            float startZ = 0;
            float length = MinSectionLength + UnityEngine.Random.value * (MaxSectionLength - MinSectionLength);

            while (startZ + length < this.Length)
            {
                if (isRight)
                {
                    RightSections.Add(new LandSection()
                    {
                        Origin = new Vector3(OVERALL_WIDTH / 2.0f - RoadFragment.OVERALL_WIDTH / 2.0f, startZ, 0),
                        Length = this.Length
                    });
                }
                else
                {
                    LeftSections.Add(new LandSection()
                    {
                        Origin = new Vector3(OVERALL_WIDTH / 2.0f + RoadFragment.OVERALL_WIDTH / 2.0f, startZ, 0),
                        Length = this.Length
                    });
                }
                startZ += length;
            }
            // the last section might be longer than the max, leave it for now.

        }

    }

    public class LandSection
    {
        public EnvironmentTile ParentEnvironmentTile;

        public Vector3 Origin;
        public float Length;
        // Width is the full width of the parent enviro tile

        public const float OVERALL_WIDTH = 40;

        public void GenerateRandom()
        {

        }

    }

   

    public class RoadFragment
    {
        // Assumed to be right in the center starting at the z=0
        public EnvironmentTile ParentEnvironmentTile;

        public float OverallLength;
        public float DrivewayWidth = 3f;
        public float NatureStripWidth = 2.0f;
        public float FootpathWidth = 2.0f;

        public const float OVERALL_WIDTH = 12;

    }



    // there are section contents.
    public class Fencing
    {
        public LandSection ParentLandSection;
        public float DrivewayEntryZ;
        public float WalkwayEntryZ;
    }

    public class PavedArea
    {
        public LandSection ParentLandSection;
        public Path2D AreaPath;
    }

    public class House
    {
        public LandSection ParentLandSection;
        public Path2D FootprintPath; // in relation to the parent land section

        public void GenerateRandom()
        {
            // generate the random 3x3 grid.


            // assume initially that the 0,0, 0,1 and 0,2 is the font of the house.
        }

    }




}
