<input.txt awk -F': ' 'function expand(rule, alts, ex, i, subs, j, altex, k, reps){if(index(rule,"\"")) return substr(rule,2,1); split(rule,alts," \\| "); ex=""; for(i in alts) { split(alts[i],subs," "); altex=""; for(j=1;j<=length(subs);j++) { if(subs[j]==8) { altex=altex "("expand(r[42])"+)" } else if(subs[j]==11) { for(k=1;k<=5;k++) {reps=(reps?reps"|":"") "("expand(r[42])"{"k"}"expand(r[31])"{"k"})" } altex=altex "("reps")"} else altex=altex expand(r[subs[j]])} ex=(ex?ex"|":"")altex } return "("ex")"} /:/{r[$1]=$2;next} /^$/ {re="^"expand(r[0])"$";print re;next} match($0,re) {print;t++} END {print t}'