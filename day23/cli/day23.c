#include <stdio.h>

#ifndef N
#define N 9
#endif

#ifndef R
#define R 100
#endif

int main(int argc, char **argv) {
  int cups[9];
  for(int i=0; i<9; i++) {
    cups[i] = argv[1][i] - '0';
  }

  int next[N+1];
  for(int i=0; i<9; i++) {
    int cup = cups[i], nextCup = cups[(i + 1)%9];
    next[cup] = nextCup;
  }
  if(N>9) {
    next[cups[8]] = 10;
    for(int i=10; i<=N; i++) {
      next[i] = (i < N) ? i + 1 : cups[0];
    }
  }

  int current = cups[0];

  for(int i=0; i<R; i++) {
#if TRACE
    int z = 1;
    for(int j=0; j<9; j++) {
      printf(z == current ? "(%d) ": "%d ", z);
      z = next[z];
    }
    printf("\n");
#endif
    int picked1 = next[current];
    int picked2 = next[picked1];
    int picked3 = next[picked2];

    int dest = current;

    while(dest == current || dest == picked1 || dest == picked2 || dest == picked3) {
      dest = dest - 1;
      if(dest < 1) dest = N;
    }
    next[current] = next[picked3];
    next[picked3] = next[dest];
    next[dest] = picked1;

    current = next[current];
  }

  printf("%d * %d = %lld", next[1], next[next[1]], (long long)next[1] * next[next[1]]);
}
