$(document).ready(function () {
    window.newgame = function () { $('#cards').html(''); }

    $('#NewGameButton').click(function () {
        $('#play').html('');
        $('#GameOptions').removeClass('hidden');
    })

    window.clearform = function ()
    {
        var bet = (parseFloat($('#bet-value').val())).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
        var current = (parseFloat($('#Credits').val()) - parseFloat($('#bet-value').val())).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
        $('#GameOptions').html('');
        $('#CurrentBet').removeClass('hidden');
        $('#CurrentBetAmount').text('$' + bet, 10);
        $('#CurrentCredits').text('$' + current, 10);
    }

    window.result = function ()
    {
        $.ajax({
            type: "Get",
            url: "/Game/Result",
            success: function (result) {
                $('#result').html(result);
                $('#PlayOptions').html('');
                $('#PlayAgain').removeClass('hidden');
            },
            error: function (result) {
                console.log(result);
            }
        });
    }

    window.hitresult = function () {
        $.ajax({
            type: "Get",
            url: "/Game/HitResult",
            success: function (result) {
                $('#result').html(result);
                if (result == "<div class='text-danger'>Lose</div>")
                {
                    $('#PlayOptions').html('');
                    $('#PlayAgain').removeClass('hidden');
                }
            },
            error: function (result) {
                console.log(result);
            }
        });
    }
});