function p(){for(i in a)if(a[i]==n)t++;delete a;n=0} BEGIN{FS=""} /./{n++;for(i=1;i<=NF;i++)a[$i]++} /^$/{p()} END{p();print t}
