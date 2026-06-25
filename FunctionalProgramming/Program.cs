using System;

// # 델리게이트 (Delegate)
// 
// 함수를 변수처럼 다룰 수 있게 해줌
// 
// e.g.
//
// - 전통적 함수 호출
//
// ```c#
// myFunc(param1, param2);
// ```
// 
// - 델리게이트 사용시
// 
// ```c#
// MyDelegate myDel = myFunc; // myFunc는 함수, 그런데 변수처럼 대입함
// myDel(param1, param2); // 함수 호출. myFunc(param1, param2); 과 완전히 동일
// ```
//
// - 델리게이트 선언

delegate void MyDelegate(int param1, int param2);

// (delegate 키워드) (반환 타입) (델리게이트명)(함수인자); 와 같이 선언
// 
// 델리게이트 변수에 할당하기
//
// 일단 MyFunc 함수 선언
class MyDelegateTest
{
    // param1과 param2를 더한 값을 출력하는 함수
    void MyFunc(int param1, int param2)
    {
        Console.WriteLine(param1 + param2);
    }

    // 변수 할당은?
    public static void AssignDelegate()
    {
        MyDelegateTest myCls = new MyDelegateTest();

        // MyFunc라는 함수를 MyDelegate타입의 변수에 저장한다
        // 주의사항: 함수 Signature가 매칭되어야 함. 리턴 타입과 파라미터 타입, 개수.
        MyDelegate myDel = myCls.MyFunc;

        Console.WriteLine("MyDelegateTest");

        // 함수 실행은 myDel변수를 함수를 호출하듯이 `변수명()` 함.
        myDel(15, 20); // print 35
    }
}

// # 델리게이트 심화
//
// ## 연산자 오버로딩 사용 (+=, -=)

class MyDelegateOperatorTest
{
    void MyAddFunc(int param1, int param2)
    {
        Console.WriteLine(param1 + param2);
    }

    void MySubtractFunc(int param1, int param2)
    {
        Console.WriteLine(param1 - param2);
    }

    public static void DelegateChain()
    {
        MyDelegateOperatorTest myCls = new MyDelegateOperatorTest();
        MyDelegate myDel = myCls.MyAddFunc;

        // 델리게이트 체인(Chain): 델리게이트를 여러 개 연결하여 체인처럼 사용하는 것.
        myDel += myCls.MySubtractFunc;

        // 주의: MyDelegate myDel = myCls.MyAddFunc + myCls.MySubtractFunc; 이건 안됨; + - 연산자는
        // 오버로딩되있지 않기 때문에.

        Console.WriteLine("MyDelegateOperatorTest");

        // 실행한다면?
        myDel(10, 20); // print 30, -10

        // 델리게이트에 바인딩된 MyAddFunc와 MySubtractFunc가 모두 실행됨.
        //
        // BUT, 델리게이트 체이닝의 결점?
        // 
        // What if, 체이닝된 델리게이트가 리턴 값이 있다면?
    }

    delegate int MyReturnDelegate(int param1, int param2);

    int MyReturnAddFunc(int param1, int param2)
    {
        Console.WriteLine("Called MyReturnAddFunc");
        return param1 + param2;
    }

    int MyReturnSubtractFunc(int param1, int param2)
    {
        Console.WriteLine("Called MyReturnSubtractFunc");
        return param1 - param2;
    }

    public static void DelegateChainWithReturn()
    {
        MyDelegateOperatorTest myCls = new MyDelegateOperatorTest();
        MyReturnDelegate myReturnDel = myCls.MyReturnAddFunc;
        myReturnDel += myCls.MyReturnSubtractFunc;

        // 예상: MyReturnAddFunc 리턴값과 MyReturnSubtractFunc 리턴값이 배열로 반환되나?
        //
        // int[] result = myReturnDel(10, 20); 일단 이건 안 됨.
        //
        // 그러면 리턴값들이 모두 더한 값이 반환되나?
        int result = myReturnDel(10, 20);

        Console.WriteLine(result); // print -10

        // 결과(매우중요): 델리게이트 체인의 마지막 함수 반환값만 리턴됨 !!

        // 그러면 델리게이트 체인에 존재하는 모든 함수의 리턴값에 접근할 방법은 없나?
        //
        // 있음, GetInvocationList()는 델리게이트 체인에 존재하는 개별 델리게이트를 배열로 반환함.

        int chain = 1;

        foreach (MyReturnDelegate myReturnDelChain in myReturnDel.GetInvocationList())
        {
            int chainResult = myReturnDelChain(10, 20);
            Console.WriteLine($" Chain{chain++} result: {chainResult}");
        } // print 30, -10

        // 이러면 Return값을 순회하여 얻을 수 있음.
    }
}

// # 델리게이트는 주로 어디에 사용하나?
//
// 델리게이트는 주로 **호출 타이밍을 알 수 없는**경우, 그 기능을 먼저 구현해놓고 함수의 호출은
// 다른 모듈에게 위임하는 용도로 사용함. 예를 들어, 사용자 입력같은 기능은 개발자가 그 호출 타이밍을
// 예단할 수 없음. 따라서 입력이 눌렸을 때의 기능을 미리 구현해 놓고 이 함수의 호출은 사용자 입력
// 매니저와 같은 클래스에 호출 시점을 위임할 수 있음.

class A
{
    public A()
    {
        // 클래스 생성시 InputManager에 버튼 눌렸을 때의 이벤트 등록
        InputManager.ButtonCallback += ButtonPressA;
    }

    public void ButtonPressA(int button)
    {
        Console.WriteLine(button + " pressed callback in class A");
    }
}

class B
{
    public B()
    {
        // 클래스 생성시 InputManager에 버튼 눌렸을 때의 이벤트 등록
        InputManager.ButtonCallback += ButtonPressB;
    }

    public void ButtonPressB(int button)
    {
        Console.WriteLine(button + " pressed callback in class B");
    }
}

// event 키워드: delegate의 접근 지정자로 대입 연산과 호출을 외부에서 접근하지 못하도록 막아놓음.
//               따라서 += 혹은 -= 연산을 통해 델리게이트 체인 제거만 가능함.

class InputManager
{
    public delegate void OnButtonPress(int button);

    // event 델리게이트로 선언
    public static event OnButtonPress ButtonCallback;
    public static void DelegatePracticalUse()
    {
        // 이 패턴은, 버튼을 눌렀을 때의 기능을 가진 클래스 A와 B가 InputManager에
        // 버튼이 눌렸을 때 자신들의 함수를 호출해 달라고 미리 약속하는 방법임. 클래스 A와 B에
        // 기능을 미리 구현하여 놓고, 함수 호출 타이밍은 InputManager에 위임하는 것.
        A a = new A();
        B b = new B();

        // 사용자가 버튼1을 눌렀을 상황 가정.
        ButtonCallback(1);

        // 사용자가 버튼2를 눌렀을 상황 가정.
        ButtonCallback(2);

        // 그냥 a.ButtonPressA(); b.ButtonPressB(); 하면 안 됨?
        //
        // 위 예시에서는 이 패턴의 이점이 부각되지 않으나, 프로젝트가 거대해지고 A B와 같은 클래스
        // 가 수없이 많이 생긴다면, InputManager에서 이들을 일일히 호출해야 함.
        //
        // 델리게이트를 사용하면 A와 B클래스에서 "버튼 눌렸을 때"에 호출될 함수를 InputManager에
        // 등록할 수 있음. 이러면 InputManager에서 클래스 A와 B의 존재에 대해 알 필요가 없고
        // 추후 A B 클래스에 수정사항이 생기더라도 InputManager에서는 그것을 반영할 필요가 없게됨.
        // 이는 코드 결합도를 제거하기 위해 보편화된 모던 소프트웨어 디자인 방법론임.
    }
}

class Program
{
    static void Main()
    {
        MyDelegateTest.AssignDelegate();
        MyDelegateOperatorTest.DelegateChain();
        MyDelegateOperatorTest.DelegateChainWithReturn();
        InputManager.DelegatePracticalUse();
    }
}
