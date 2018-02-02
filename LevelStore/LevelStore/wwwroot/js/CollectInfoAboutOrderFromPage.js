function CollectInfo(order) {
    try {
        order.firstName = $($("[name='FirstName']")[0]).val();
        order.lastname = $($("[name='LastName']")[0]).val();
        order.Address = $($("[name='Address']")[0]).val();
        order.City = $($("[name='City']")[0]).val();
        order.Email = $($("[name='Email']")[0]).val();
        order.NovaPoshta = $($("[name='NovaPoshta']")[0]).val();
        order.Phone = $($("[name='Phone']")[0]).val();
        order.Comment = $($("[name='Comment']")[0]).val();
        order.Status = $($("[name='Status']")[0]).val();
        for (var i = 0; i < order.Lines.length; i++) {
            order.Lines[i].CartLineID = $($("[name='Lines[" + i + "].CartLineID']")[0]).val();
            order.Lines[i].Quantity = $($("[name='Lines[" + i + "].Quantity']")[0]).val();
        }
    } catch (err) {
        return null;
    }
    return order;
}