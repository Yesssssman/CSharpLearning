using System.Collections;

// IEnumerable: 순회 가능한 객체. 
//
// IEnumerator: 순회자. C++의 Iter역할을 함
//
// 자료 구조에 대한 공통적인 순회를 위해 추상화된 인터페이스들. 순회하기 위해서 대표적으로 foreach문을 사용함.

class Program
{
    static void Main()
    {
        int[] array = { 1, 2, 3, 4, 5 };

        // Array도 IEnumerable 상속함.
        Console.WriteLine(array is IEnumerable); // True

        // Array가 IEnumerable이므로 foreach문에서 사용 가능.
        foreach (int i in array)
        {
            Console.Write(i + " "); // 1 2 3 4 5
        }

        Console.WriteLine();

        // IEnumerator 얻어오기
        IEnumerator enumerator = array.GetEnumerator();

        // # IEnumerator 사용법
        //
        // for문에서 요소에 접근하기 위해 주로 사용하는 "int i" 변수라고 생각하면 됨. 다만 카운팅 자체를 숫자 연산이 아닌 추상화된 함수로 수행하는 것.
        //
        // .MoveNext(): i++. 리턴값으로 bool형을 반환하는데 다음 요소가 없으면 false 리턴.
        //
        // .Current: array[i]. 현재 Enumerator가 가르키는 요소를 반환함.
        //
        // .Reset(): i = -1; Enumerator가 가르키는 요소를 시작 인덱스로 초기화.

        while (enumerator.MoveNext()) // MoveNext() 가 False이면 종료 ( == 다음 요소가 없으면 종료)
        {
            Console.WriteLine(enumerator.Current); // enumerator가 가르키는 값 출력
        }

        enumerator.Reset(); // 리셋 (i = -1;)
        enumerator.MoveNext();

        Console.WriteLine(enumerator.Current); // 1
    }
}
