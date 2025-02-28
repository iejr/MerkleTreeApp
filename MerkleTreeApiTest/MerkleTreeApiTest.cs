using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using MerkleTreeApi.Controllers;
using MerkleTreeApi.Models;
using MerkleTreeApi.Services;

namespace MerkleTreeApiTest;

public class MerkleTreeApiTest
{
    [Fact]
    public void Create_ReturnsOkResult_WithCorrectRootHash() {
        Mock<IUserStateService> mockUserStateService = new Mock<IUserStateService>();
        mockUserStateService.Setup(s => s.user_state).Returns(new Dictionary<int, User>());
        mockUserStateService.Setup(s => s.merkle_tree).Returns(new MerkleTree.MerkleTree());

        UserBalanceController controller = new UserBalanceController(mockUserStateService.Object);

        CreateRequest request = new CreateRequest {
            user_data = [
                new User { id = 1, balance = 1111 },
                new User { id = 2, balance = 2222 },
                new User { id = 3, balance = 3333 },
                new User { id = 4, balance = 4444 },
                new User { id = 5, balance = 5555 },
                new User { id = 6, balance = 6666 },
                new User { id = 7, balance = 7777 },
                new User { id = 8, balance = 8888 },
            ],
            leaf_tag = "ProofOfReserve_Leaf",
            branch_tag = "ProofOfReserve_Branch",
        };
        var result = controller.Create(request) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        CreateResponse expect = new CreateResponse {
            root_hash = "b1231de33da17c23cebd80c104b88198e0914b0463d0e14db163605b904a7ba3",
        };

        var actual = result.Value as dynamic;
        Assert.Equal(JsonSerializer.Serialize(expect), JsonSerializer.Serialize(actual));
    }

    [Fact]
    public void GetBalance_ReturnsOkResult_WithCorrectMerkleProof() {
        Mock<IUserStateService> mockUserStateService = new Mock<IUserStateService>();
        mockUserStateService.Setup(s => s.user_state).Returns(
            new Dictionary<int, User>() {
                {
                    1,
                    new User { id = 1, balance = 1111 }
                },
                {
                    2,
                    new User { id = 2, balance = 2222 }
                },
            }
        );

        MerkleTree.MerkleTree tree = new MerkleTree.MerkleTree();
        tree.Build(["(1,1111)", "(2,2222)"], "leaf", "branch");
        mockUserStateService.Setup(s => s.merkle_tree).Returns(tree);

        UserBalanceController controller = new UserBalanceController(mockUserStateService.Object);

        GetBalanceRequest request = new GetBalanceRequest {
            user_id = 2,
        };
        var result = controller.GetBalance(request) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var expect = new GetBalanceResponse {
            balance = 2222,
            proofs = [
                new MerkleTree.MerkleProofNode {
                    hex_hash = "b723d835092b192a9af84df34d20b5f27bea8b7b1015bfeb6e3178c55579b35a",
                    pos = MerkleTree.MerkleTreeNode.ChildPosition.Right,
                },
                new MerkleTree.MerkleProofNode {
                    hex_hash = "9eeb55bd94c28650c8e136c46b057c9d0c3b95fbadebfb5d0ac4b2d01b64830e",
                    pos = MerkleTree.MerkleTreeNode.ChildPosition.Left,
                },
            ],
        };

        var actual = result.Value as dynamic;
        Assert.Equal(JsonSerializer.Serialize(expect), JsonSerializer.Serialize(actual));
    }
    
    [Fact]
    public void GetBalance_ReturnsUserNotFoundResult_WithCorrectMessage() {
        Mock<IUserStateService> mockUserStateService = new Mock<IUserStateService>();
        mockUserStateService.Setup(s => s.user_state).Returns(new Dictionary<int, User>());
        mockUserStateService.Setup(s => s.merkle_tree).Returns(new MerkleTree.MerkleTree());

        UserBalanceController controller = new UserBalanceController(mockUserStateService.Object);

        GetBalanceRequest request = new GetBalanceRequest {
            user_id = 2233,
        };
        var result = controller.GetBalance(request) as NotFoundObjectResult;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("User Id not found", result.Value);
    }
}