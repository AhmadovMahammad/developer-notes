using System;

public class AdoNet
{
	public AdoNet()
	{
		using var connection = new SqlConnection();
	}
}
