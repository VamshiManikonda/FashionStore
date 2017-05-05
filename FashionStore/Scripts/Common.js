
function DirtyKeypressEventForAlphaNumeralsTextBoxes(id, e, alphanumerals) {
    //  alphanumerals = { 0 - Alphabetic and Numeric, 1 - Only Numeric, 2 - Only Alphabetic}
    
    if (!$('#' + id).hasClass('dirty')) {
        switch (alphanumerals) {
            case 0:
                if ((e <= 57 && e >= 48) || (e <= 90 && e >= 65) || (e <= 122 && e >= 97) || e == 8 || e == 46) {
                    $('#' + id).addClass('dirty');
                }
                break;
            case 1:
                if ((e <= 57 && e >= 48) || e == 8 || e == 46) {
                    $('#' + id).addClass('dirty');
                }
                break;
            case 2:
                if ((e <= 90 && e >= 65) || e == 8 || e == 46) {
                    $('#' + id).addClass('dirty');
                }
                break;
            default:
                break;
        }
    }
}