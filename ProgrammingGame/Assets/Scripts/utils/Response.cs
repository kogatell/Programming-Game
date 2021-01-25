using UnityEngine;

public class Response<T> where T : class
{
	private bool ready = false;
	private T data = null;

	public T Data => data;

	public bool Ready => ready;
	
	public void SetReady(T resp)
	{
		ready = true;
		data = resp;
	}
	
}
