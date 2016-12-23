using System.Collections.Generic;

namespace AdventOfCode2016.Days
{
	public class Day01 : Day
	{
		public Day01(string input)
			: base(01, input)
		{
		}
		
		public override object RunPart1()
		{
			int x=0,y=0,b=0,s;
			foreach(var a in (" "+Input).Split(',')){
				s=int.Parse(a.Substring(2));
				b=(b+(a[1]+3)%4)%4;
				if(b%2<1)x+=b==0?-s:s;
				else y+=b==1?-s:s;
			}
			return(x<0?-x:x)+(y<0?-y:y);
		}

		public override object RunPart2()
		{
			int x=0,y=0,b=0,i;
			var k=new HashSet<string>();
			foreach(var a in (" "+Input).Split(',')){
				b=(b+(a[1]+3)%4)%4;
				for(i=0;i++<int.Parse(a.Substring(2));){
					if(b%2<1)x+=b==0?-1:1;
					else y+=b==1?-1:1;
					if(!k.Add($"{x}|{y}"))
						return(x<0?-x:x)+(y<0?-y:y);
				}
			}
			return 0;
		}
	}
}