namespace BlueBird.World
{
	public enum EntityType_enu : byte {
		InvisibleData, // No graphics
		Ground,
		Vegetation,
		Structure,
		Animal,
		Monster,
		Character,
		Climate, // Weather phenomenon and climate effects
		Count // Count is just a nice trick to get the enum length
	}
}