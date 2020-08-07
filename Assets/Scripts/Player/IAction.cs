public interface IAction
{
    // 시작하는 함수와 끝내는 함수가 필요
    // Begin의 인수로 다양한 객체들을 담기 위해 object 자료형 사용
    void Begin(object initValue);
    void End();
}
