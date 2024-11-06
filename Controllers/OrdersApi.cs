using Microsoft.AspNetCore.Mvc;
using AbuHelmy_Buisness;
using System.Data;
namespace Abu_HelmyWebAPI.Controllers
{
    [ApiController]
    [Route("api/abu_helmy/orders")]
    public class OrdersApi : ControllerBase
    {


        [HttpGet("{date}", Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DataTable>>> GetAllOrders(DateTime date)
        {
            var orders = await clsOrder.GetAllOrderWithDetailsInDateDay(date);
            if (orders == null)
                return NotFound("no orders found is this date");
            return Ok(orders);
        }

        [HttpGet("{id}", Name = "GetOrderByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<clsOrder>> GetOrderByID(int id)
        {
            if (id <= 0)
            {

                return BadRequest("You shoure id big than zero");
            }
            var order = await clsOrder.GetOrderInfoByIDAsync(id);

            if (order == null)
                return NotFound("this studen not found: {id}");

            return Ok(order);

        }

        [HttpPut("{id}", Name = "UpdateAgentName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UpdateAgentName(int id, string agentName)
        {
            if (id <= 0 || agentName == null)
            {

                return BadRequest("You mast shoure id big than zero");
            }
            if (await clsOrder.UpdateAgentName(id, agentName))
            {

                return Ok("save update agentName");
            }
            return NotFound("not found agent with this id");
        }

        [HttpGet(Name = "IsClientNameIsExistInDateDay")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> IsClientNameIsExistInDateDay(string name, DateTime date)
        {

            if (string.IsNullOrEmpty(name))
            {

                return BadRequest("You mast shoure name ");
            }
            if (await clsOrder.IsClientNameIsExistInDateDay(name, date))
            {

                return Ok("the client name is exist");
            }
            return NotFound("not found client name ");
        }

        [HttpDelete("{id}", Name = "deleteOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteAgentName(int id)
        {
            if (id <= 0)
            {

                return BadRequest("You mast shoure id big than zero");
            }
            if (await clsOrder.DeleteOrder(id))
            {
                return Ok("deleted order");
            }
            return NotFound("not found order");
        }

        [HttpGet("{id}", Name = "GetOrderWithDetailsByOrderID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DataTable>>> GetOrderWithDetailsByOrderID(int id)
        {
            if (id <= 0)
            {

                return BadRequest("You mast shoure id is big than zero");
            }
            var order = await clsOrder.GetOrderWithDetailsByOrderID(id);
            if (order == null)
            {
                return Ok(order);
            }
            return NotFound("not found order with this id");
        }


        [HttpPost(Name = "AddNewOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<clsOrder>> AddNewOrder(clsOrder newOrder)
        {
            if (newOrder.OrderDetails == null || string.IsNullOrEmpty(newOrder.ClientName) || string.IsNullOrEmpty(newOrder.AgentName))
            {
                return BadRequest("this bad request");
            }

            if (await newOrder.Save())
            {
                var order = await clsOrder.GetOrderInfoByIDAsync(newOrder.OrderID);
                return CreatedAtRoute("newOrder", order);
            }
            return BadRequest("bad request");
        }



        [HttpPut("{id}", Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<clsOrder>> UpdateOrder(int id, clsOrder updateOrder)
        {
            if (id > 0 || updateOrder.OrderDetails == null || updateOrder.OrderID >= 0 || string.IsNullOrEmpty(updateOrder.ClientName) ||
                string.IsNullOrEmpty(updateOrder.AgentName))
            {
                return BadRequest("bad requset");
            }

            var order = await clsOrder.GetOrderInfoByIDAsync(id);
            order.OrderDetails = updateOrder.OrderDetails;
            order.ClientName = updateOrder.ClientName;
            order.AgentName = updateOrder.AgentName;
            order.OrderDate = updateOrder.OrderDate;
            order.OrderID = id;
            order.Note = updateOrder.Note;
            order.CreatedByUserID = updateOrder.CreatedByUserID;

            if (await order.Save())
            {
                return Ok(order);
            }
            return NotFound("not found");
        }


    }
}
