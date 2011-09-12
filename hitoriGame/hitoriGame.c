
#include <stdio.h>
#include <stdlib.h>

int __hitoriGameA( int *number, int remain, int count );
int __hitoriGameB( int *number, int remain, int count );

int
___hitoriGame( int *number, int remain, int count )
{
    int i;
    int countA   = 0;
    int countB   = 0;
    int *numberA = (int *)malloc( sizeof ( int ) * remain );
    int *numberB = (int *)malloc( sizeof ( int ) * remain );

    if ( !numberA || !numberB )
        exit( 255 );

    for ( i = 0; i < remain; i++ ) {
        numberA[i] = number[i];
        numberB[i] = number[i];
    }

    /*   (1a) 全部の数を2で割る */
    countA = __hitoriGameA( numberA, remain, count + 1 );

    /*   (1b) 5 の倍数を取り除く*/
    countB = __hitoriGameB( numberB, remain, count + 1 );

    count = min( countA, countB );

    free( numberB );
    free( numberA );

    return ( count );
}

int
__hitoriGameA( int *number, int remain, int count )
{
    int i;
    int check = 0;

    /* 全部の数を2で割る */
    for ( i = 0; i < remain; i++ )
        number[i] /= 2;

    /* 全部の数が5の倍数なら、全部取り除いて終了 */
    for ( i = 0; i < remain; i++ ) {
        if ( number[i] % 5 == 0 )
            check++;
    }
    if ( check == remain )
        count++;    /* 終了 */
    else {
        /* 5 の倍数ではないものが含まれている場合  */
        if ( check ) {
            /* (1) 5 の倍数が1個でも含まれている場合 */
            count = ___hitoriGame( number, remain, count );
        }
        else {
            /* (2) 5 の倍数が1個も含まれない場合 */
            /*   (2a) 全部の数を2で割る */
            count = __hitoriGameA( number, remain, count + 1 );
        }
    }

    return ( count );
}

int
__hitoriGameB( int *number, int remain, int count )
{
    int i;
    int check = 0;

    /* 5 の倍数を取り除く*/
    for ( i = 0; i < remain; i++ ) {
        if ( number[i] % 5 == 0 ) {
            int j;
            for ( j = i; j < remain - 1; j++ )
                number[j] = number[j + 1];
            remain--;
            i--;
        }
    }

    /* 5 の倍数が1個も含まれない場合 */
    /*   全部の数を2で割る */
    count = __hitoriGameA( number, remain, count + 1 );

    return ( count );
}

int
_hitoriGameC( const int *origNumber, int numOfNumbers )
{
    int count  = 0;
    int remain = numOfNumbers;
    int check  = 0;
    int i;
    int *number = (int *)malloc( sizeof ( int ) * numOfNumbers );

    if ( !number )
        exit( 255 );
    for ( i = 0; i < numOfNumbers; i++ )
        number[i] = origNumber[i];

    /* 本体 */
    check = 0;

    /* 全部の数が5の倍数なら、全部取り除いて終了 */
    for ( i = 0; i < remain; i++ ) {
        if ( number[i] % 5 == 0 )
            check++;
    }
    if ( check == remain )
        count++;    /* 終了 */
    else {
        /* 5 の倍数ではないものが含まれている場合  */
        if ( check ) {
            /* (1) 5 の倍数が1個でも含まれている場合 */
            count = ___hitoriGame( number, remain, count );
        }
        else {
            /* (2) 5 の倍数が1個も含まれない場合 */
            /*   (2a) 全部の数を2で割る          */
            count = __hitoriGameA( number, remain, count + 1 );
        }
    }

    free( number );

    return ( count );
}



void
_hitoriGame( int *number, int numOfNumbers )
{
    int count = _hitoriGameC( number, numOfNumbers );
    printf( "%d\n", count );
}

void
hitoriGame( FILE *fp )
{
    char    buf[BUFSIZ];
    char    *p;
    int     level        = 0;
    int     currentGame  = 0;
    int     numOfGames   = 0;
    int     numOfNumbers = 0;
    int     *numbers;


    while ( ( p = fgets( buf, BUFSIZ - 1, fp ) ) != NULL ) {
        while ( (*p == ' ') || (*p == '\t') )
            p++;
        if ( level == 0 ) {
            /* 1st */
            numOfGames = atoi( p );
            if ( numOfGames <= 0 )
                break;
            level++;
            continue;
        }

        if ( level == 1 ) {
            /* 2nd */
            numOfNumbers = atoi( p );
            if ( numOfNumbers < 0 )
                break;
            if ( ++currentGame > numOfGames )
                break;
            level++;
            continue;
        }

        if ( level == 2 ) {
            /* 3rd */
            int i;
            numbers = (int *)malloc( sizeof ( int ) * numOfNumbers );
            if ( !numbers )
                break;

            for ( i = 0; i < numOfNumbers; i++ ) {
                while ( (*p == ' ') || (*p == '\t') )
                    p++;

                numbers[i] = atoi( p );

                while ( (*p != ' ') && (*p != '\t') )
                    p++;
                if ( !(*p) || (*p == '\n') )
                    break;
            }

            if ( i == numOfNumbers )
                _hitoriGame( numbers, numOfNumbers );

            free( numbers );
            level--;
        }
    }
}


int
main( int argc, char **argv )
{
    if ( argc < 1 )
        hitoriGame( stdin );
    else {
        FILE    *fp = NULL;
        int     i;
        for ( i = 1; i < argc; i++ ) {
            fp = fopen( (const char *)(argv[i]), "r" );
            if ( fp ) {
                hitoriGame( fp );
                fclose( fp );
            }
        }
    }

    return ( 0 );
}

