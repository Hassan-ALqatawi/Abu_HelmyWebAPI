using AbuHelmy_Buisness;
using AbuHelmy_DataAccess;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Abu_HelmyWebAPI.Controllers
{
    [ApiController]
    [Route("api/abu_helmy/orderDetails")]
    public class OrderDetailsWebApi : ControllerBase
    {

        [HttpGet("{orderID}",Name ="getAllOrderDetailsByOrderID")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<clsOrderDetailsData.clsOrderDetailsDTO>> GetAllOrderDetailsByOrderID(int orderID)
        {
            if (orderID >= 0)
            {
                return BadRequest("this is bad request");
            }
            var orderDetails = await clsOrderDetails.GetAllOrderDetailsByOrderIDAsync(orderID);
            if (orderDetails == null)
            {
                return NotFound("not found ");
            }
            return Ok(orderDetails);
        }


        [HttpPost(Name = "AddNewOrderDetails")]
        [ProducesResponseType (StatusCodes.Status201Created)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<clsOrderDetailsData.clsOrderDetailsDTO>> AddNewOrderDetails(clsOrderDetails orderDetails)
        {
            if ( orderDetails.OrderID >= 0 || orderDetails.Quantity == 0 || orderDetails.BirdTypeName == 0)
            {
                return BadRequest("this is bad request");
            }
            if(await orderDetails.Save())
            {
                return CreatedAtRoute("GetNewOrderDetails",GetAllOrderDetailsByOrderID(orderDetails.OrderID));
            }
            return BadRequest("not save order details");
        }



        [HttpPut("{id}",Name ="UpdateOrderDetails")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        public async Task<ActionResult<clsOrderDetailsData.clsOrderDetailsDTO>> UpdateOrderDetails(int id, clsOrderDetails updeteOrderDetails)
        {
            if (id < 0 || updeteOrderDetails.OrderID < 0 || updeteOrderDetails.BirdTypeName <= 0 || updeteOrderDetails.Quantity < 0)
            {
                return BadRequest("bad request");
            }
            var orderDetails = await GetAllOrderDetailsByOrderID(updeteOrderDetails.OrderID);
            
            if (orderDetails != null)
            {
                return Ok(orderDetails);
            }
            return NotFound("not found");
        }




        [HttpDelete("{orderID}",Name = "DeleteOrderDetailsByOrderID")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteOrderDetailsByOrderID(int orderID)
        {
            if (orderID >= 0)
            {
                return BadRequest("this is bad request");
            }
            if (await clsOrderDetails.DeleteOrderDetailsByOrderIDAsync(orderID))
            {
                return Ok("delete order details");
            }
            return NotFound("not found");
        }

    }
}
