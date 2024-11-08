
#include <iostream>

int main(){
    if(__cplusplus == 202002L)
        std::cout << "C++20" << std::endl;
    else if(__cplusplus == 201703L)
        std::cout << "C++17" << std::endl;
    else if(__cplusplus == 201402L)
        std::cout << "C++14" << std::endl;

    std::cout << "Hello" << std::endl;

    return 0;
}