using System;
using System.Collections;

// # yield 키워드
// 
// IEnumerable을 리턴하는 함수에서 요소를 리턴하는 흐름을 제어할 수 있게 만든 키워드

class Program
{
    // max값보다 작은 i의 제곱수들을 IEnumerable로 리턴. yield 키워드 사용.
    static IEnumerable<int> MyYieldFunc(int i, int max)
    {
        int sqr = i;
        int num = i;

        while (true)
        {
            num *= sqr;

            Console.WriteLine($"MyYieldFunc print: Next calculated {num}");

            // yield return: 값을 리턴하고, 함수의 실행을 **일시정지** 시킨다. IEnumerable.MoveNext()를 다시 호출하면 마지막으로 일시정지한
            // 시점으로부터 함수를 다시 시작한다.
            if (num <= max) yield return num;
            // yield break: 리턴값 없이 함수를 종료한다. IEnumerable.MoveNext()는 False를 반환.
            else yield break;
        }
    }

    static void Main()
    {
        IEnumerable<int> enumerable = MyYieldFunc(2, 64);

        foreach (int i in enumerable)
        {
            // 여기서 MyYieldFunc 함수의 출력문과 foreach의 출력문을 비교해보면 서로의 출력문이 교대로 출력됨.
            // yield는 함수의 실행을 **정지**시키기 때문에 foreach문이 다시 MoveNext()를 호출하기 전까지 MyYieldFunc
            // 의 출력문은 출력되지 않는다.
            Console.WriteLine($"foreach print: Next calculated {i}");
        }

        // Reset is unsupported: 함수의 흐름을 기준으로 다음 값에 접근하기 때문에 Reset() 함수를 쓸 수 없다.
        // 함수 실행을 되감을 수는 없기 때문!
        // enumerable.GetEnumerator().Reset(); error!

        Console.WriteLine("Hello, World!");
    }
}