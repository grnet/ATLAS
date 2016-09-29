function ChangeBotShieldInput(Obj) {
    var change = 0;
    var Str = Obj.value;
    var StrL = Str.length;
    var StrOut = '';

    var c = ' ';
    var cOut = ' ';

    for (var i = 0; i < StrL; i++) {
        c = Str.substring(i, i + 1);
        cOut = '';

        if ((c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9')) { cOut = c; }
        if (c == 'Α' || c == 'a' || c == 'α' || c == 'ά') { change = 1; cOut = 'A'; }
        if (c == 'Β' || c == 'b' || c == 'β') { change = 1; cOut = 'B'; }
        if (c == 'Γ' || c == 'g' || c == 'γ') { change = 1; cOut = 'G'; }
        if (c == 'Δ' || c == 'd' || c == 'δ') { change = 1; cOut = 'D'; }
        if (c == 'Ε' || c == 'e' || c == 'ε' || c == 'έ') { change = 1; cOut = 'E'; }
        if (c == 'Ζ' || c == 'z' || c == 'ζ') { change = 1; cOut = 'Z'; }
        if (c == 'Η' || c == 'h' || c == 'η' || c == 'ή') { change = 1; cOut = 'H'; }
        if (c == 'Θ' || c == 'u' || c == 'θ') { change = 1; cOut = 'U'; }
        if (c == 'Ι' || c == 'Ϊ' || c == 'i' || c == 'ι' || c == 'ί' || c == 'ϊ' || c == 'ΐ') { change = 1; cOut = 'I'; }
        if (c == 'Κ' || c == 'k' || c == 'κ') { change = 1; cOut = 'K'; }
        if (c == 'Λ' || c == 'l' || c == 'λ') { change = 1; cOut = 'L'; }
        if (c == 'Μ' || c == 'm' || c == 'μ') { change = 1; cOut = 'M'; }
        if (c == 'Ν' || c == 'n' || c == 'ν') { change = 1; cOut = 'N'; }
        if (c == 'Ξ' || c == 'j' || c == 'ξ') { change = 1; cOut = 'J'; }
        if (c == 'Ο' || c == 'o' || c == 'ο' || c == 'ό') { change = 1; cOut = 'O'; }
        if (c == 'Π' || c == 'p' || c == 'π') { change = 1; cOut = 'P'; }
        if (c == 'Ρ' || c == 'r' || c == 'ρ') { change = 1; cOut = 'R'; }
        if (c == 'Σ' || c == 's' || c == 'σ') { change = 1; cOut = 'S'; }
        if (c == 'Τ' || c == 't' || c == 'τ') { change = 1; cOut = 'T'; }
        if (c == 'Υ' || c == 'Ϋ' || c == 'y' || c == 'υ' || c == 'ύ' || c == 'ϋ' || c == 'ΰ') { change = 1; cOut = 'Y'; }
        if (c == 'Φ' || c == 'f' || c == 'φ') { change = 1; cOut = 'F'; }
        if (c == 'Χ' || c == 'x' || c == 'χ') { change = 1; cOut = 'X'; }
        if (c == 'Ψ' || c == 'c' || c == 'ψ') { change = 1; cOut = 'C'; }
        if (c == 'Ω' || c == 'v' || c == 'ω' || c == 'ώ') { change = 1; cOut = 'V'; }
        if (c == 'Q' || c == 'q') { change = 1; cOut = 'Q'; }
        if (c == 'w' || c == 'ς') { change = 1; cOut = 'W'; }
        if (c == ';') { change = 1; cOut = 'Q'; }
        if (cOut == '') { change = 1; }

        StrOut = StrOut + cOut;
    };

    if (change == 1) Obj.value = StrOut;
    return;
}

function ValidateBotShieldInput(s, e) {
    e.IsValid = e.Value != '';
    s.errormessage = DEFAULT_BS_ERROR_MESSAGE;
    if (e.IsValid) {
        if (e.Value.length != BS_MAX_LENGTH) {
            e.IsValid = false;
            s.errormessage = Resources.Botshield_Validation + BS_MAX_LENGTH;
        }
    }
}