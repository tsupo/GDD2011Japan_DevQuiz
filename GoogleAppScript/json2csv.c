
#include <stdio.h>
#include <stdlib.h>
#include <string.h>


void
json2csv( FILE *fp )
{
    char    buf[65536 + 1];
    char    *p, *q;
    int     usage;
    int     capacity;
    char    filename[_MAX_PATH];
    FILE    *gp = NULL;

    memset( buf, 0x00, 65536 + 1 );
    fread( buf, sizeof ( char ), 65536, fp );

    p = buf;
    while ( *p ) {
        if ( !strncmp( p, "\"city_name\":", 12 ) ) {
            if ( gp )
                fclose( gp );
            p += 12;
            q = strchr( p, '"' );
            if ( q ) {
                p = q + 1;
                q = strchr( p, '"' );
                if ( q ) {
                    strncpy( filename, p, q - p );
                    filename[q - p] = '\0';
                    strcat( filename, ".csv" );
                    gp = fopen( filename, "w" );

                    p = q + 1;
                }
            }
        }
        else if ( !strncmp( p, "\"usage\":", 8 ) ) {
            p += 8;
            while ( (*p < '0') || (*p > '9') )
                p++;
            usage = atoi( p );
            while ( (*p >= '0') && (*p <= '9') )
                p++;
        }
        else if ( !strncmp( p, "\"capacity\":", 11 ) ) {
            p += 11;
            while ( (*p < '0') || (*p > '9') )
                p++;
            capacity = atoi( p );
            while ( (*p >= '0') && (*p <= '9') )
                p++;

            fprintf( gp, "%d, %d\n", capacity, usage );
        }

        p++;
    }

    if ( gp )
        fclose( gp );
}

int
main( int argc, char **argv )
{
    if ( argc < 1 )
        json2csv( stdin );
    else {
        FILE    *fp = NULL;
        int     i;
        for ( i = 1; i < argc; i++ ) {
            fp = fopen( (const char *)(argv[i]), "r" );
            if ( fp ) {
                json2csv( fp );
                fclose( fp );
            }
        }
    }

    return ( 0 );
}
